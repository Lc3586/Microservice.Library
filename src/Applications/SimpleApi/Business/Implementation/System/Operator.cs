using Business.Interface.System;
using Library.Container;
using Microsoft.AspNetCore.Http;
using Model.Common;
using Model.Utils.SampleAuthentication.SampleAuthenticationDTO;
using System.Linq;

namespace Business.Implementation.System
{
    /// <summary>
    /// 操作者
    /// </summary>
    public class Operator : IOperator, IDependency
    {
        #region DI

        public Operator(
            IHttpContextAccessor httpContextAccessor,
            IAuthoritiesBusiness authoritiesBusiness,
            IUserBusiness userBusiness,
            IMemberBusiness memberBusiness)
        {
            AuthoritiesBusiness = authoritiesBusiness;
            UserBusiness = userBusiness;
            MemberBusiness = memberBusiness;

            IsAuthenticated = httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

            if (IsAuthenticated)
                AuthenticationInfo = new AuthenticationInfo()
                {
                    Id = httpContextAccessor.HttpContext.User.Claims?.FirstOrDefault(o => o.Type == nameof(AuthenticationInfo.Id))?.Value,
                    UserType = httpContextAccessor.HttpContext.User.Claims?.FirstOrDefault(o => o.Type == nameof(AuthenticationInfo.UserType))?.Value,
                    Account = httpContextAccessor.HttpContext.User.Claims?.FirstOrDefault(o => o.Type == nameof(AuthenticationInfo.Account))?.Value,
                    Nickname = httpContextAccessor.HttpContext.User.Claims?.FirstOrDefault(o => o.Type == nameof(AuthenticationInfo.Nickname))?.Value,
                    HeadimgUrl = httpContextAccessor.HttpContext.User.Claims?.FirstOrDefault(o => o.Type == nameof(AuthenticationInfo.HeadimgUrl))?.Value
                };
        }

        readonly IAuthoritiesBusiness AuthoritiesBusiness;
        readonly IUserBusiness UserBusiness;
        readonly IMemberBusiness MemberBusiness;

        #endregion

        #region 公共

        /// <summary>
        /// 是否已登录
        /// </summary>
        public bool IsAuthenticated { get; }

        /// <summary>
        /// 当前操作者身份验证信息
        /// </summary>
        public AuthenticationInfo AuthenticationInfo { get; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public OperatorUserInfo UserInfo
        {
            get
            {
                switch (AuthenticationInfo?.UserType)
                {
                    case Model.System.UserType.系统用户:
                        return UserBusiness.GetOperatorDetail(AuthenticationInfo.Id);
                    case Model.System.UserType.会员:
                        return MemberBusiness.GetOperatorDetail(AuthenticationInfo.Id);
                    default:
                        //throw new ApplicationException("登录信息异常: 用户类型错误.");
                        return null;
                }
            }
        }

        /// <summary>
        /// 判断是否为管理员
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin => AuthenticationInfo.UserType == Model.System.UserType.系统用户 && AuthoritiesBusiness.IsAdminUser(AuthenticationInfo.Id);

        #endregion
    }
}
