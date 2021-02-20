using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Library.SampleAuthentication.Extension
{
    /// <summary>
    /// 简易身份验证服务处理接口
    /// </summary>
    public interface IAuthenticationHandler
    {
        /// <summary>
        /// 登录
        /// </summary>
        Task Login(HttpContext context);

        /// <summary>
        /// 注销
        /// </summary>
        Task Logout(HttpContext context);

        /// <summary>
        /// 禁止访问
        /// </summary>
        Task AccessDenied(HttpContext context);
    }
}
