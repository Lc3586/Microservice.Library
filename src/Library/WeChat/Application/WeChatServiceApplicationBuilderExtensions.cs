using System;
using System.Collections.Generic;
using System.Text;
using Library.WeChat.Application;
using Library.WeChat.Extension;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 使用中间件<see cref="WeChatNotifyV3Middleware"/>的扩展方法
    /// </summary>
    public static class WeChatServiceApplicationBuilderExtensions
    {
        /// <summary>
        /// 使用微信通知中间件
        /// <para>V3版本</para>
        /// </summary>
        /// <param name="app"></param>
        /// <param name="handler">处理类</param>
        /// <exception cref="WeChatNotifyException"></exception>
        /// <returns></returns>
        public static IApplicationBuilder UseWeChatNotifyV3(this IApplicationBuilder app, IWeChatNotifyHandler handler)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            return app.UseMiddleware<WeChatNotifyV3Middleware>(handler);
        }
    }
}
