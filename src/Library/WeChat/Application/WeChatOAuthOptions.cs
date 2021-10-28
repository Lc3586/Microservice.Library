using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System.Collections.Generic;

namespace Microservice.Library.WeChat.Application
{
    /// <summary>
    /// 微信授权配置
    /// </summary>
    public class WeChatOAuthOptions
    {
        /// <summary>
        /// 站点根地址
        /// </summary>
        /// <remarks>key: HttpScheme,value: url</remarks>
        public Dictionary<string, string> WebRootUrl { get; set; }

        /// <summary>
        /// 获取Code接口
        /// </summary>
        public string AuthorizeUrl { get; set; }

        /// <summary>
        /// code换取网页授权access_token接口
        /// </summary>
        public string AccessTokenUrl { get; set; }

        /// <summary>
        /// 拉取用户信息接口
        /// </summary>
        public string UserInfoUrl { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        /// <remarks>默认值 zh_CN</remarks>
        public string Language { get; set; } = "zh_CN";

        /// <summary>
        /// 授权接口
        /// </summary>
        /// <remarks>
        /// <para>scope为snsapi_base</para>
        /// <para>使用<see cref="Extension.WeChatOAuthV2Middleware"/>中间件时，中间件监听的接口。</para>
        /// </remarks>
        public PathString OAuthBaseUrl { get; set; }

        /// <summary>
        /// 授权接口
        /// </summary>
        /// <remarks>
        /// <para>scope为snsapi_userinfo</para>
        /// <para>使用<see cref="Extension.WeChatOAuthV2Middleware"/>中间件时，中间件监听的接口。</para>
        /// </remarks>
        public PathString OAuthUserInfoUrl { get; set; }
    }
}
