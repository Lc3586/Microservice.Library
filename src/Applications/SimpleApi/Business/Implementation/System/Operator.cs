
using Library.Container;
using Library.Http;
using Library.Models;
using Library.Log;
using System.Linq;
using Library.Extension;
using Business.Interface;
using Business.Interface.System;
using Library.WebApp;
using Model.System.UserDTO;
using Model.System;

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
            ILogger logger)
        {
            AuthoritiesBusiness = authoritiesBusiness;
            Logger = logger;
        }

        readonly IAuthoritiesBusiness AuthoritiesBusiness;//= AutofacHelper.GetScopeService<IAuthoritiesBusiness>();

        readonly ILogger Logger;//= AutofacHelper.GetScopeService<ILogger>();

        #endregion

        #region 公共

        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        public string UserId => HttpContextCore.Current.User.Claims?.FirstOrDefault(o => o.Type == "id")?.Value;

        /// <summary>
        /// 用户信息
        /// </summary>
        public Authorities Property => AuthoritiesBusiness.GetUser(UserId, false, false, false);

        /// <summary>
        /// 判断是否为管理员
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin => AuthoritiesBusiness.IsAdminUser(UserId);

        #endregion
    }
}
