using Library.Extension;
using Library.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.CAS.CASDTO;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Api.Controllers.Utils
{
    /// <summary>
    /// CAS认证接口
    /// </summary>
    [Route("/cas")]
    [CheckModel]
    [SwaggerTag("CAS认证接口")]
    public class CASController : BaseApiController
    {
        HttpContext Context;

        public CASController(IHttpContextAccessor accessor)
        {
            Context = accessor.HttpContext;
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <remarks>
        /// <para>验证后重定向至指定地址</para>
        /// <para>未登录时将会先重定向至登录</para>
        /// <para>身份信息将会附加在重定向地址之后（?casInfo=【身份信息JSON序列化字符串】）</para>
        /// <para>未指定地址时将直接输出身份信息</para>
        /// </remarks>
        /// <param name="returnUrl">重定向地址</param>
        /// <returns>身份信息</returns>
        [HttpGet("authorize")]
        public async Task Authorize(string returnUrl)
        {
            var info = new AuthorizedInfo()
            {
                AppName = Config.ProjectName,
                Id = Context.User.Claims?.FirstOrDefault(o => o.Type == "id")?.Value,
                Account = Context.User.Claims?.FirstOrDefault(o => o.Type == "account")?.Value,
                Name = Context.User.Claims?.FirstOrDefault(o => o.Type == "name")?.Value
            };

            if (returnUrl.IsNullOrEmpty())
                await Context.Response.WriteAsync(info.ToJson());
            else
                await Task.Run(() => Context.Response.Redirect($"{returnUrl}?casInfo={UrlEncoder.Default.Encode(info.ToJson())}"));
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <remarks>
        /// <para>使用此接口判断登录状态</para>
        /// <para>已登录返回身份信息</para>
        /// <para>未登录返回状态码401</para>
        /// </remarks>
        /// <returns>身份信息</returns>
        [HttpPost("authorized")]
        public async Task<AuthorizedInfo> Authorized()
        {
            var info = new AuthorizedInfo()
            {
                AppName = Config.ProjectName,
                Id = Context.User.Claims?.FirstOrDefault(o => o.Type == "id")?.Value,
                Account = Context.User.Claims?.FirstOrDefault(o => o.Type == "account")?.Value,
                Name = Context.User.Claims?.FirstOrDefault(o => o.Type == "name")?.Value,
                //CasToken = Context.Request.Cookies[".Cas"]
            };

            return await Task.FromResult(info);
        }

        /// <summary>
        /// 拒绝访问
        /// </summary>
        /// <returns></returns>
        [HttpGet("access-denied")]
        [AllowAnonymous]
        public async Task AccessDenied()
        {
            if (Context.User.Identity.IsAuthenticated)
                await Task.Run(() => Context.Response.Redirect("/cas/authorize"));
            else
                await Context.Response.WriteAsync("拒绝访问");
        }

        /// <summary>
        /// 登录异常
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExternalLoginFailure")]
        [AllowAnonymous]
        public async Task ExternalLoginFailure()
        {
            if (Context.User.Identity.IsAuthenticated)
                await Task.Run(() => Context.Response.Redirect("/cas/authorize"));
            else
                await Context.Response.WriteAsync("系统繁忙");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="returnUrl">登录后重定向地址</param>
        /// <returns></returns>
        [HttpGet("login")]
        [AllowAnonymous]
        public async Task Login(string returnUrl)
        {
            if (returnUrl?.ToLower().IndexOf("/cas/login") >= 0)
                returnUrl = null;

            var props = new AuthenticationProperties { RedirectUri = returnUrl };
            await Context.ChallengeAsync("CAS", props);
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="returnUrl">注销后重定向地址</param>
        /// <param name="logoutCAS">单点注销（当前登录的所有应用都会注销）</param>
        /// <param name="tgt"></param>
        /// <returns></returns>
        [HttpGet("logout")]
        public async Task LogOut(string returnUrl, bool logoutCAS = false, string tgt = null)
        {
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = $"{Config.WebRootUrl}/cas/login";

            await LogOut(tgt);

            if (logoutCAS)
                Context.Response.Redirect($"{Config.CASBaseUrl}/logout?service={returnUrl}");
            else
                Context.Response.Redirect(returnUrl);
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="tgt"></param>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task LogOut(string tgt = null)
        {
            await Context.SignOutAsync();
        }

        /// <summary>
        /// 获取验证后的身份信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("getToken")]
        public async Task<string> GetToken()
        {
            return await Task.FromResult(JWTHelper.GetToken(new AuthorizedInfo
            {
                AppName = Config.ProjectName,
                Id = Context.User.Claims?.FirstOrDefault(o => o.Type == "id")?.Value,
                Account = Context.User.Claims?.FirstOrDefault(o => o.Type == "account")?.Value,
                Name = Context.User.Claims?.FirstOrDefault(o => o.Type == "name")?.Value
            }.ToJson(), Config.JWTSecret));
        }
    }
}
