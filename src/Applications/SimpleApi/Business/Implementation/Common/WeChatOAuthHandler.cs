using AutoMapper;
using Business.Interface.Common;
using Business.Interface.System;
using Entity.Common;
using FreeSql;
using Library.Container;
using Library.DataMapping.Gen;
using Library.FreeSql.Gen;
using Library.WeChat.Extension;
using Microsoft.AspNetCore.Http;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Threading.Tasks;

namespace Business.Implementation.Common
{
    /// <summary>
    /// 微信用户信息业务类
    /// </summary>
    public class WeChatOAuthHandler : IWeChatUserInfoBusiness, IWeChatOAuthHandler, IDependency
    {
        #region DI

        public WeChatOAuthHandler(
            IFreeSqlProvider freeSqlProvider,
            IAutoMapperProvider autoMapperProvider,
            IOperationRecordBusiness operationRecordBusiness)
        {
            Orm = freeSqlProvider.GetFreeSql();
            Repository = Orm.GetRepository<Common_WeChatUserInfo, string>();
            Mapper = autoMapperProvider.GetMapper();
            OperationRecordBusiness = operationRecordBusiness;
        }

        #endregion

        #region 私有成员

        IFreeSql Orm { get; set; }

        IBaseRepository<Common_WeChatUserInfo, string> Repository { get; set; }

        IMapper Mapper { get; set; }

        IOperationRecordBusiness OperationRecordBusiness { get; set; }

        #endregion

        #region 公共

        public async Task Handler(HttpContext context, string appId, string openId, string scope)
        {
            if (Repository.Where(o => o.AppId == appId && o.OpenId == openId).Any())
            {
                if (!Repository.Where(o => o.AppId == appId && o.OpenId == openId && o.Scope == scope).Any())
                {
                    if (Repository.UpdateDiy.Set(o => o.Scope, scope)
                         .Where(o => o.OpenId == openId)
                         .ExecuteAffrows() <= 0)
                        throw new ApplicationException($"更新用户作用域失败, \r\n\topenId: {openId}, \r\n\tscope: {scope}.");
                }

                if(Repository.Where(o => o.AppId == appId && o.OpenId == openId && o.Users).Any())

                return;
            }


            var entity = new Common_WeChatUserInfo
            {

            };
            Repository.Insert();
            throw new NotImplementedException();
        }

        public async Task Handler(HttpContext context, string appId, OAuthUserInfo userinfo)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
