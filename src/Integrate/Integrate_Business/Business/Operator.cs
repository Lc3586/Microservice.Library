using Integrate_Business.Config;
using Library.Container;
using Library.Http;
using Library.Models;
using Library.Log;
using Library.WebApp;
using Integrate_Model.System;
using System.Linq;
using Library.Extention;

namespace Integrate_Business
{
    /// <summary>
    /// 操作者
    /// </summary>
    public class Operator : IOperator, IDependency
    {
        #region DI

        private IBase_UserBusiness _sysUserBus { get => AutofacHelper.GetScopeService<IBase_UserBusiness>(); }
        public ILogger Logger { get => AutofacHelper.GetScopeService<ILogger>(); }

        #endregion

        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        public string UserId
        {
            get
            {
                return HttpContextCore.Current.User.Claims?.FirstOrDefault(o => o.Type == "id")?.Value;
            }
        }

        public UserDTO Property { get => _sysUserBus.GetTheData(UserId); }

        #region 操作方法

        /// <summary>
        /// 判断是否为超级管理员
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin()
        {
            return Property.Roles.Any_Ex(o => o.Type.HasFlag(RoleType.superAdminRole));
        }

        #endregion
    }
}
