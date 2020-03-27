using Integrate_Business.Config;
using Integrate_Business.Util;
using Library.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integrate_Api.Controllers
{
    /// <summary>
    /// CAS认证接口
    /// </summary>
    [CheckModel]
    //[Route("/[controller]/[action]")]
    public class CASController : Controller
    {
        HttpContext Context;

        public CASController(IHttpContextAccessor accessor)
        {
            Context = accessor.HttpContext;
        }

        /// <summary>
        /// 跳转登录
        /// </summary>
        /// <param name="returnUrl">登录后跳转地址</param>
        /// <returns></returns>
        [HttpGet("casLogin")]
        [AllowAnonymous]
        public async Task Login(string returnUrl)
        {
            if (SystemConfig.systemConfig.casCustom)
            {
                Context.Response.Redirect(SystemConfig.systemConfig.casCustomloginUrl + "?service=" + returnUrl);
                await Task.FromResult(0);
            }
            else
            {
                var props = new AuthenticationProperties { RedirectUri = returnUrl };
                await Context.ChallengeAsync("CAS", props);
            }
        }

        /// <summary>
        /// 拒绝访问
        /// </summary>
        /// <returns></returns>
        [HttpGet("casAccess-Denied")]
        public async Task<object> AccessDenied()
        {
            return await Task.FromResult("拒绝访问");
        }

        /// <summary>
        /// 获取验证后的身份信息
        /// 未登录时为null
        /// </summary>
        /// <returns></returns>
        [HttpGet("casAuthorized")]
        public async Task<CASModel.AuthorizedInfo> Authorized()
        {
            if (!Context.User.Identity.IsAuthenticated)
                return await Task.FromResult<CASModel.AuthorizedInfo>(null);
            return await Task.FromResult(new
                 CASModel.AuthorizedInfo()
            {
                Id = Context.User.Claims?.FirstOrDefault(o => o.Type == "id")?.Value,
                Account = Context.User.Claims?.FirstOrDefault(o => o.Type == "account")?.Value,
                Name = Context.User.Claims?.FirstOrDefault(o => o.Type == "name")?.Value
            });
        }

        /// <summary>
        /// 提交登录
        /// </summary>
        /// <param name="getTGT"></param>
        /// <returns></returns>
        [HttpPost("casSubmitLogin")]
        [AllowAnonymous]
        public async Task<string> SubmitLogin(CASModel.GetTGT getTGT)
        {
            return await CASHelper.GetTGT(getTGT);
        }

        /// <summary>
        /// 获取票据
        /// </summary>
        /// <param name="getST"></param>
        /// <returns></returns>
        [HttpPost("casGetTicket")]
        public async Task<string> GetTicket(CASModel.GetST getST)
        {
            return await CASHelper.GetST(getST);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="getUserInfo"></param>
        /// <returns></returns>
        [HttpPost("casGetUserInfo")]
        public async Task<CASModel.UserInfo> GetUserInfo(CASModel.GetUserInfo getUserInfo)
        {
            return await CASHelper.GetUserInfo(getUserInfo);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="logOut"></param>
        /// <returns></returns>
        [HttpGet("casLogout")]
        public async Task LogOut(CASModel.LogOut logOut)
        {
            if (SystemConfig.systemConfig.casCustom)
            {
                await CASHelper.DeleteTGT(logOut);
            }
            else
            {
                if (logOut?.LogoutCAS == true)
                    Context.Response.Redirect($"{SystemConfig.systemConfig.casBaseUrl}/logout");
                else
                    await Context.SignOutAsync();
            }
            if (!string.IsNullOrEmpty(logOut.ReturnUrl))
                Context.Response.Redirect(logOut.ReturnUrl);
        }
    }
}
