using Model.Common;
using Model.Utils.SampleAuthentication.SampleAuthenticationDTO;

namespace Business.Interface.System
{
    /// <summary>
    /// 操作者
    /// </summary>
    public interface IOperator
    {
        /// <summary>
        /// 是否已登录
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// 身份验证信息
        /// </summary>
        AuthenticationInfo AuthenticationInfo { get; }

        /// <summary>
        /// 用户信息
        /// </summary>
        OperatorUserInfo UserInfo { get; }

        /// <summary>
        /// 判断是否为超级管理员
        /// </summary>
        /// <returns></returns>
        bool IsSuperAdmin { get; }

        /// <summary>
        /// 判断是否为管理员
        /// </summary>
        /// <returns></returns>
        bool IsAdmin { get; }
    }
}
