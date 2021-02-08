using AutoMapper;
using Business.Interface.Common;
using Business.Interface.System;
using Business.Utils;
using Business.Utils.Pagination;
using Entity.Common;
using Entity.Public;
using Entity.System;
using FreeSql;
using Library.Cache;
using Library.Container;
using Library.DataMapping.Gen;
using Library.FreeSql.Extention;
using Library.FreeSql.Gen;
using Library.OpenApi.Extention;
using Library.WeChat.Extension;
using Microsoft.AspNetCore.Http;
using Model.Common;
using Model.Common.WeChatUserInfoDTO;
using Model.System.Pagination;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Implementation.Common
{
    /// <summary>
    /// 微信用户信息业务类
    /// </summary>
    public class WeChatOAuthHandler : BaseBusiness, IWeChatUserInfoBusiness, IWeChatOAuthHandler, IDependency
    {
        #region DI

        public WeChatOAuthHandler(
            IFreeSqlProvider freeSqlProvider,
            IAutoMapperProvider autoMapperProvider,
            IOperationRecordBusiness operationRecordBusiness,
            IMemberBusiness memberBusiness,
            IUserBusiness userBusiness)
        {
            Orm = freeSqlProvider.GetFreeSql();
            Repository = Orm.GetRepository<Common_WeChatUserInfo, string>();
            Repository_User = Orm.GetRepository<System_User, string>();
            Repository_Member = Orm.GetRepository<Public_Member, string>();
            Mapper = autoMapperProvider.GetMapper();
            OperationRecordBusiness = operationRecordBusiness;
            MemberBusiness = memberBusiness;
            UserBusiness = userBusiness;
        }

        #endregion

        #region 私有成员

        IFreeSql Orm { get; set; }

        IBaseRepository<Common_WeChatUserInfo, string> Repository { get; set; }

        IBaseRepository<System_User, string> Repository_User { get; set; }

        IBaseRepository<Public_Member, string> Repository_Member { get; set; }

        IMapper Mapper { get; set; }

        IOperationRecordBusiness OperationRecordBusiness { get; set; }

        IMemberBusiness MemberBusiness { get; set; }

        IUserBusiness UserBusiness { get; set; }

        /// <summary>
        /// 创建微信用户信息
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="openId"></param>
        /// <param name="scope"></param>
        void CreateWeChatUserInfo(string appId, string openId, string scope)
        {
            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                var entity = new Common_WeChatUserInfo
                {
                    AppId = appId,
                    OpenId = openId,
                    Scope = scope,
                    Enable = false
                }.InitEntityWithoutOP();

                Repository.Insert(entity);

                var orId = OperationRecordBusiness.Create(new Common_OperationRecord
                {
                    DataType = nameof(Common_WeChatUserInfo),
                    DataId = entity.Id,
                    Explain = $"创建微信用户信息[AppId {entity.AppId}, OpenId {entity.OpenId}]."
                }, false);
            });

            if (!success)
                throw new ApplicationException("创建微信用户信息失败.", ex);
        }

        /// <summary>
        /// 更新微信用户信息
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="userinfo"></param>
        void UpdateWeChatUserInfo(string appId, OAuthUserInfo userinfo)
        {
            if (Repository.UpdateDiy
                 .Where(o => o.AppId == appId && o.OpenId == userinfo.openid)
                 .Set(o => o.Nickname, userinfo.nickname)
                 .Set(o => o.HeadimgUrl, userinfo.headimgurl)
                 .Set(o => o.Sex, userinfo.sex)
                 .Set(o => o.Country, userinfo.country)
                 .Set(o => o.Province, userinfo.province)
                 .Set(o => o.City, userinfo.city)
                 .Set(o => o.Enable, true)
                 .Set(o => o.ModifyTime, DateTime.Now)
                 .ExecuteAffrows() <= 0)
                throw new ApplicationException($"更新微信用户信息失败, \r\n\tAppId: {appId}, \r\n\tOpenId: {userinfo.openid}.");
        }

        /// <summary>
        /// 自动创建用户
        /// </summary>
        /// <param name="userinfo"></param>
        void CreateUser(OAuthUserInfo userinfo)
        {
            UserBusiness.Create(new Model.System.UserDTO.Create
            {
                Account = $"user_{Repository_User.Select.Count():000000}",
                Nickname = userinfo.nickname,
                Enable = true,
                HeadimgUrl = userinfo.headimgurl,
                Remark = "通过微信自动创建用户账号."
            }, false);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="userinfo"></param>
        void UpdateUser(OAuthUserInfo userinfo)
        {
            UserBusiness.Edit(new Model.System.UserDTO.Edit
            {
                Nickname = userinfo.nickname,
                HeadimgUrl = userinfo.headimgurl
            }, false);
        }

        /// <summary>
        /// 自动创建会员
        /// </summary>
        /// <param name="userinfo"></param>
        void CreateMember(OAuthUserInfo userinfo)
        {
            MemberBusiness.Create(new Model.Public.MemberDTO.Create
            {
                Account = $"member_{Repository_User.Select.Count():000000000}",
                Nickname = userinfo.nickname,
                HeadimgUrl = userinfo.headimgurl,
                Sex = userinfo.sex == 1 ? "男" : userinfo.sex == 2 ? "女" : null,
                Enable = true,
                Remark = "通过微信自动创建会员账号."
            }, false);
        }

        /// <summary>
        /// 更新会员
        /// </summary>
        /// <param name="userinfo"></param>
        void UpdateMember(OAuthUserInfo userinfo)
        {
            MemberBusiness.Edit(new Model.Public.MemberDTO.Edit
            {
                Nickname = userinfo.nickname,
                HeadimgUrl = userinfo.headimgurl,
                Sex = userinfo.sex == 1 ? "男" : userinfo.sex == 2 ? "女" : null
            }, false);
        }

        #endregion

        #region 公共

        public List<List> GetList(PaginationDTO pagination)
        {
            var entityList = Orm.Select<Common_WeChatUserInfo>()
                                .GetPagination(pagination)
                                .ToList<Common_WeChatUserInfo, List>(typeof(List).GetNamesWithTagAndOther(true, "_List"));

            var result = Mapper.Map<List<List>>(entityList);

            return result;
        }

        public Detail GetDetail(string id)
        {
            var entity = Repository.GetAndCheckNull(id);

            var result = Mapper.Map<Detail>(entity);

            result._Users = Repository_User.Where(o => o.WeChatUserInfos.AsSelect().Where(p => p.OpenId == result.OpenId).Any())
                                .ToDtoList<System_User, Model.System.UserDTO.Detail>(typeof(Model.System.UserDTO.Detail).GetNamesWithTagAndOther(true, "_List"));

            result._Members = Repository_Member.Where(o => o.WeChatUserInfos.AsSelect().Where(p => p.OpenId == result.OpenId).Any())
                                .ToDtoList<Public_Member, Model.Public.MemberDTO.Detail>(typeof(Model.Public.MemberDTO.Detail).GetNamesWithTagAndOther(true, "_List"));

            return result;
        }

        public string GetState(StateInfo data)
        {
            var state = IdHelper.NextIdString().Replace("-", "");
            CacheHelper.Cache.SetCache(state, data, TimeSpan.FromMinutes(20), ExpireType.Absolute);
            return state;
        }

        public async Task Handler(HttpContext context, string appId, string openId, string scope, string state = null)
        {
            if (string.IsNullOrWhiteSpace(state))
                throw new ApplicationException("state参数不可为空.");

            if (!CacheHelper.Cache.ContainsKey(state))
                throw new ApplicationException("state参数已过期.");

            var stateInfo = CacheHelper.Cache.GetCache<StateInfo>(state);

            if (!Repository.Where(o => o.AppId == appId && o.OpenId == openId).Any())
            {
                CreateWeChatUserInfo(appId, openId, scope);
            }
            else if (!Repository.Where(o => o.AppId == appId && o.OpenId == openId && o.Scope == scope).Any())
            {
                //    if (Repository.UpdateDiy
                //         .Set(o => o.Scope, scope)
                //         .Where(o => o.AppId == appId && o.OpenId == openId)
                //         .ExecuteAffrows() <= 0)
                //        throw new ApplicationException($"更新微信用户作用域失败, \r\n\topenId: {openId}, \r\n\tscope: {scope}.");
            }
            else if (Repository.Where(o => o.AppId == appId && o.OpenId == openId
                 && (
                     (stateInfo.Type == WeChatStateType.系统用户登录 && o.Users.AsSelect().Any())
                     || (stateInfo.Type == WeChatStateType.会员登录 && o.Members.AsSelect().Any())))
                 .Any())
            {
                if (stateInfo.Type == WeChatStateType.系统用户登录)
                    UserBusiness.Login(openId);
                else if (stateInfo.Type == WeChatStateType.会员登录)
                    MemberBusiness.Login(openId);
                else
                    throw new ApplicationException("用户类型错误.");

                context.Response.Redirect(stateInfo.RedirectUrl);

                return;
            }

            context.Response.Redirect($"{Config.WeChatService.OAuthUserInfoUrl}?state={state}");
            await Task.FromResult(true);
        }

        public async Task Handler(HttpContext context, string appId, OAuthUserInfo userinfo, string state = null)
        {
            if (!Repository.Where(o => o.AppId == appId && o.OpenId == userinfo.openid).Any())
                throw new ApplicationException("微信信息不存在或已被移除.");

            if (string.IsNullOrWhiteSpace(state))
                throw new ApplicationException("state参数不可为空.");

            if (!CacheHelper.Cache.ContainsKey(state))
                throw new ApplicationException("state参数已过期.");

            UpdateWeChatUserInfo(appId, userinfo);

            var stateInfo = CacheHelper.Cache.GetCache<StateInfo>(state);

            switch (stateInfo.Type)
            {
                case WeChatStateType.系统用户登录:
                    CreateUser(userinfo);
                    UserBusiness.Login(userinfo.openid);
                    break;
                case WeChatStateType.会员登录:
                    CreateMember(userinfo);
                    MemberBusiness.Login(userinfo.openid);
                    break;
                case WeChatStateType.更新系统用户微信信息:
                    UpdateUser(userinfo);
                    break;
                case WeChatStateType.更新会员微信信息:
                    UpdateMember(userinfo);
                    break;
                default:
                    break;
            }

            context.Response.Redirect(stateInfo.RedirectUrl);
            await Task.FromResult(true);
        }

        #endregion
    }
}
