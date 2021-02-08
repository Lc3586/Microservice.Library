using Business.Interface.System;
using Library.Container;
using Library.WebApp;
using Model.Common;
using System;
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
            IAuthoritiesBusiness authoritiesBusiness,
            IUserBusiness userBusiness,
            IMemberBusiness memberBusiness)
        {
            AuthoritiesBusiness = authoritiesBusiness;
            UserBusiness = userBusiness;
            MemberBusiness = memberBusiness;

            UserId = HttpContextCore.Current.User.Claims?.FirstOrDefault(o => o.Type == nameof(UserId))?.Value;
            UserType = HttpContextCore.Current.User.Claims?.FirstOrDefault(o => o.Type == nameof(UserType))?.Value;
        }

        readonly IAuthoritiesBusiness AuthoritiesBusiness;//= AutofacHelper.GetScopeService<IAuthoritiesBusiness>();
        readonly IUserBusiness UserBusiness;//= AutofacHelper.GetScopeService<IUserBusiness>();
        readonly IMemberBusiness MemberBusiness;//= AutofacHelper.GetScopeService<IMemberBusiness>();

        #endregion

        #region 公共

        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public string UserType { get; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public OperatorDetail Detail
        {
            get
            {
                switch (UserType)
                {
                    case Model.System.UserType.系统用户:
                        return UserBusiness.GetOperatorDetail(UserId);
                    case Model.System.UserType.会员:
                        return MemberBusiness.GetOperatorDetail(UserId);
                    default:
                        throw new ApplicationException("登录信息异常: 用户类型错误.");
                }
            }
        }

        /// <summary>
        /// 判断是否为管理员
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin => UserType == Model.System.UserType.系统用户 && AuthoritiesBusiness.IsAdminUser(UserId);

        #endregion
    }
}
