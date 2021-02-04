using Library.WeChat.Application;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Library.WeChat.Extension
{
    /// <summary>
    /// 微信开发令牌验证中间件
    /// </summary>
    /// <exception cref="WeChatTokenVerificationException"></exception>
    public class WeChatTokenVerificationMiddleware
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="options"></param>
        public WeChatTokenVerificationMiddleware(RequestDelegate next, WeChatGenOptions options)
        {
            Next = next;
            Options = options;
            Security = new SecurityHelper(Options);
        }

        #region 私有成员

        readonly RequestDelegate Next;
        readonly WeChatGenOptions Options;
        readonly SecurityHelper Security;

        #endregion

        #region 公开方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (context.Request.Method.Equals(HttpMethod.Get.Method)
                    && context.Request.Path.HasValue)
                {
                    if (context.Request.Path.Equals(Options.WeChatDevOptions.TokenVerificationUrl))
                    {
                        var signature = context.Request.Query["signature"].ToString();
                        var timestamp = context.Request.Query["timestamp"].ToString();
                        var nonce = context.Request.Query["nonce"].ToString();
                        var echostr = context.Request.Query["echostr"].ToString();

                        var str1 = string.Join("",
                            new List<string>
                            {
                            Options.WeChatDevOptions.Token,
                            timestamp,
                            nonce
                            }.OrderBy(o => o, StringComparer.Ordinal));

                        if (Security.ToSHA1String(str1).Equals(signature))
                            await context.Response.WriteAsync(echostr).ConfigureAwait(false);
                        else
                            await context.Response.WriteAsync(null).ConfigureAwait(false);
                    }
                }

                await Next.Invoke(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new WeChatTokenVerificationException("中间件运行时发生异常.", ex);
            }
        }

        #endregion
    }
}
