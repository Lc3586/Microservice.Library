using Api.Controllers.Utils;
using Business.Interface.Common;
using Business.Interface.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;

namespace Api.Controllers
{
    /// <summary>
    /// 微信认证接口
    /// </summary>
    [Route("/wechat-oath")]
    [ApiPermission]
    [CheckModel]
    [SwaggerTag("微信认证接口")]
    public class WeChatOAuthController : BaseApiController
    {
        #region DI

        public WeChatOAuthController(
            IHttpContextAccessor httpContextAccessor,
            IWeChatUserInfoBusiness weChatUserInfoBusiness,
            IMemberBusiness memberBusiness)
        {
            HttpContextAccessor = httpContextAccessor;
            WeChatUserInfoBusiness = weChatUserInfoBusiness;
            MemberBusiness = memberBusiness;
        }

        readonly IHttpContextAccessor HttpContextAccessor;

        readonly IWeChatUserInfoBusiness WeChatUserInfoBusiness;

        readonly IMemberBusiness MemberBusiness;

        #endregion

        #region 基础接口

        /// <summary>
        /// 会员登录
        /// </summary>
        /// <param name="returnUrl">重定向地址</param>
        [HttpGet("member-login")]
        [AllowAnonymous]
        public void MemberLogin(string returnUrl)
        {
            var state = WeChatUserInfoBusiness.GetState(new Model.Common.WeChatUserInfoDTO.StateInfo
            {
                Type = Model.Common.WeChatStateType.会员登录,
                RedirectUrl = returnUrl,
                Data = new System.Collections.Generic.Dictionary<string, object>
                {
                    {
                        "AutoCreate",
                        true
                    }
                }
            });

            var url = $"{Config.WeChatService.OAuthBaseUrl}?state={state}";

            HttpContextAccessor.HttpContext.Response.Redirect(url);
        }

        /// <summary>
        /// 更新会员微信用户信息
        /// </summary>
        /// <param name="memberId">会员Id</param>
        /// <param name="returnUrl">重定向地址</param>
        [HttpGet("member-update/{memberId}")]
        [AllowAnonymous]
        public void UpdateMemberWeChatUserInfo(string memberId, string returnUrl)
        {
            var state = WeChatUserInfoBusiness.GetState(new Model.Common.WeChatUserInfoDTO.StateInfo
            {
                Type = Model.Common.WeChatStateType.微信信息同步至会员信息,
                RedirectUrl = returnUrl,
                Data = new System.Collections.Generic.Dictionary<string, object>
                {
                    {
                        "MemberId",
                        memberId
                    }
                }
            });

            var url = $"{Config.WeChatService.OAuthUserInfoUrl}?state={state}";

            HttpContextAccessor.HttpContext.Response.Redirect(url);
        }

        /// <summary>
        /// 获取用于系统用户绑定微信的链接
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="asyncUserInfo">同步微信信息至用户信息</param>
        /// <param name="returnUrl">重定向地址</param>
        /// <returns></returns>
        [HttpGet("user-bind-url/{userId}/{asyncUserInfo}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "链接地址", typeof(string))]
        public async Task<object> GetUserBindUrl(string userId, bool asyncUserInfo, string returnUrl)
        {
            var state = WeChatUserInfoBusiness.GetState(new Model.Common.WeChatUserInfoDTO.StateInfo
            {
                Type = Model.Common.WeChatStateType.系统用户绑定微信,
                RedirectUrl = returnUrl,
                Data = new System.Collections.Generic.Dictionary<string, object>
                {
                    {
                        "UserId",
                        userId
                    },
                    {
                        "AsyncUserInfo",
                        asyncUserInfo
                    }
                }
            });

            var url = $"{Config.WeChatService.OAuthUserInfoUrl}?state={state}";

            return await Task.FromResult(Success<string>(url));
        }

        /// <summary>
        /// 系统用户登录
        /// </summary>
        /// <param name="returnUrl">重定向地址</param>
        [HttpGet("user-login")]
        [AllowAnonymous]
        public void UserLogin(string returnUrl)
        {
            var state = WeChatUserInfoBusiness.GetState(new Model.Common.WeChatUserInfoDTO.StateInfo
            {
                Type = Model.Common.WeChatStateType.系统用户登录,
                RedirectUrl = returnUrl,
                Data = new System.Collections.Generic.Dictionary<string, object>
                {
                    {
                        "AutoCreate",
                        true
                    }
                }
            });

            var url = $"{Config.WeChatService.OAuthBaseUrl}?state={state}";

            HttpContextAccessor.HttpContext.Response.Redirect(url);
        }

        /// <summary>
        /// 更新系统用户微信用户信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="returnUrl">重定向地址</param>
        [HttpGet("user-update/{userId}")]
        [AllowAnonymous]
        public void UpdateUserWeChatUserInfo(string userId, string returnUrl)
        {
            var state = WeChatUserInfoBusiness.GetState(new Model.Common.WeChatUserInfoDTO.StateInfo
            {
                Type = Model.Common.WeChatStateType.微信信息同步至系统用户信息,
                RedirectUrl = returnUrl,
                Data = new System.Collections.Generic.Dictionary<string, object>
                {
                    {
                        "UserId",
                        userId
                    }
                }
            });

            var url = $"{Config.WeChatService.OAuthUserInfoUrl}?state={state}";

            HttpContextAccessor.HttpContext.Response.Redirect(url);
        }

        #endregion

        #region 拓展接口



        #endregion
    }
}
