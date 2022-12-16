using System;
using System.Collections.Generic;
using System.Text;
using Microservice.Library.WeChat.Application;
using Microservice.Library.WeChat.Extension;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 使用微信中间件的扩展方法
    /// </summary>
    public static class WeChatServiceApplicationBuilderExtensions
    {
        /// <summary>
        /// 使用微信通知中间件
        /// <para>V3版本</para>
        /// </summary>
        /// <param name="app"></param>
        /// <exception cref="WeChatNotifyException"></exception>
        /// <remarks>需要先将<see cref="IWeChatNotifyHandler"/>注册到容器中</remarks>
        /// <returns></returns>
        public static IApplicationBuilder UseWeChatNotifyV3(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            return app.UseMiddleware<WeChatNotifyV3Middleware>();
        }

        /// <summary>
        /// 使用微信开发令牌验证中间件
        /// </summary>
        /// <param name="app"></param>
        /// <exception cref="WeChatTokenVerificationException"></exception>
        /// <returns></returns>
        public static IApplicationBuilder UseWeChatTokenVerification(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            return app.UseMiddleware<WeChatTokenVerificationMiddleware>();
        }

        /// <summary>
        /// 使用微信网页授权中间件
        /// </summary>
        /// <param name="app"></param>
        /// <exception cref="WeChatOAuthException"></exception>
        /// <remarks>需要先将<see cref="IWeChatOAuthHandler"/>注册到容器中</remarks>
        /// <returns></returns>
        public static IApplicationBuilder UseWeChatOAuthV2(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            return app.UseMiddleware<WeChatOAuthV2Middleware>();
        }
    }
}
