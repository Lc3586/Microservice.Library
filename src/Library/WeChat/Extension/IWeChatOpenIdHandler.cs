using Microservice.Library.WeChat.Model;
using Microsoft.AspNetCore.Http;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Library.WeChat.Extension
{
    /// <summary>
    /// 微信网页授权处理接口
    /// </summary>
    public interface IWeChatOAuthHandler
    {
        /// <summary>
        /// 处理用户公众号唯一标识
        /// </summary>
        /// <remarks>建议处理后进行重定向</remarks>
        /// <param name="context">当前请求上下文</param>
        /// <param name="appId">公众号标识</param>
        /// <param name="openId">用户公众号唯一标识</param>
        /// <param name="scope">用户授权的作用域, 使用逗号（,）分隔</param>
        /// <param name="state">url中附带的state参数</param>
        Task Handler(HttpContext context, string appId, string openId, string scope, string state = null);

        /// <summary>
        /// 处理用户基础信息
        /// </summary>
        /// <remarks>建议处理后进行重定向</remarks>
        /// <param name="context">当前请求上下文</param>
        /// <param name="appId">公众号标识</param>
        /// <param name="userinfo">用户基础信息</param>
        /// <param name="state">url中附带的state参数</param>
        Task Handler(HttpContext context, string appId, OAuthUserInfo userinfo, string state = null);
    }
}
