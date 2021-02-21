using Business.Interface.System;
using Library.Extension;
using Library.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Utils.SampleAuthentication.SampleAuthenticationDTO;
using Model.System;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers.Utils
{
    /// <summary>
    /// 简易身份验证接口
    /// </summary>
    [Route("/sa")]
    [CheckModel]
    [SwaggerTag("简易身份验证接口")]
    public class SampleAuthenticationController : BaseApiController
    {
        public SampleAuthenticationController(
            IHttpContextAccessor accessor,
            IUserBusiness userBusiness)
        {
            Context = accessor.HttpContext;
            UserBusiness = userBusiness;
        }

        readonly HttpContext Context;

        readonly IUserBusiness UserBusiness;

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
        public async Task<AuthenticationInfo> Authorized()
        {
            var info = new AuthenticationInfo()
            {
                Id = Context.User.Claims?.FirstOrDefault(o => o.Type == nameof(AuthenticationInfo.Id))?.Value,
                UserType = Context.User.Claims?.FirstOrDefault(o => o.Type == nameof(AuthenticationInfo.UserType))?.Value,
                Account = Context.User.Claims?.FirstOrDefault(o => o.Type == nameof(AuthenticationInfo.Account))?.Value,
                Nickname = Context.User.Claims?.FirstOrDefault(o => o.Type == nameof(AuthenticationInfo.Nickname))?.Value,
                Sex = Context.User.Claims?.FirstOrDefault(o => o.Type == nameof(AuthenticationInfo.Sex))?.Value,
                HeadimgUrl = Context.User.Claims?.FirstOrDefault(o => o.Type == nameof(AuthenticationInfo.HeadimgUrl))?.Value
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
                await Task.Run(() => Context.Response.Redirect("/sa/login"));
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
                await Task.Run(() => Context.Response.Redirect("/sa/login"));
            else
                await Context.Response.WriteAsync("系统繁忙");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="data">参数</param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<object> Login(LoginRequest data)
        {
            var authenticationInfo = UserBusiness.Login(data.Account, data.Password);

            var claims = new List<Claim>
            {
                new Claim(nameof(AuthenticationInfo.Id), authenticationInfo.Id),
                new Claim(nameof(AuthenticationInfo.UserType), authenticationInfo.UserType),
                new Claim(nameof(AuthenticationInfo.Account), authenticationInfo.Account),
                new Claim(nameof(AuthenticationInfo.Nickname), authenticationInfo.Nickname ?? string.Empty),
                new Claim(nameof(AuthenticationInfo.Sex), authenticationInfo.Sex ?? string.Empty),
                new Claim(nameof(AuthenticationInfo.HeadimgUrl), authenticationInfo.HeadimgUrl ?? string.Empty)
            };

            await Context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

            return Success("登录成功");
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="data">参数</param>
        /// <returns></returns>
        [HttpGet("logout")]
        public async Task Logout(LogoutRequest data)
        {
            if (data?.ReturnUrl?.ToLower().IndexOf("/sa/logout") >= 0)
                data.ReturnUrl = null;

            var props = new AuthenticationProperties { RedirectUri = data.ReturnUrl };

            await Context.SignOutAsync(props);
        }

        /// <summary>
        /// 获取验证后的身份信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("getToken")]
        public async Task<string> GetToken()
        {
            return await Task.FromResult(JWTHelper.GetToken(new AuthenticationInfo
            {
                Id = Context.User.Claims?.FirstOrDefault(o => o.Type == nameof(AuthenticationInfo.Id))?.Value,
                UserType = Context.User.Claims?.FirstOrDefault(o => o.Type == nameof(AuthenticationInfo.UserType))?.Value,
                Account = Context.User.Claims?.FirstOrDefault(o => o.Type == nameof(AuthenticationInfo.Account))?.Value,
                Nickname = Context.User.Claims?.FirstOrDefault(o => o.Type == nameof(AuthenticationInfo.Nickname))?.Value,
                Sex = Context.User.Claims?.FirstOrDefault(o => o.Type == nameof(AuthenticationInfo.Sex))?.Value,
                HeadimgUrl = Context.User.Claims?.FirstOrDefault(o => o.Type == nameof(AuthenticationInfo.HeadimgUrl))?.Value
            }.ToJson(), Config.JWTSecret));
        }
    }
}
