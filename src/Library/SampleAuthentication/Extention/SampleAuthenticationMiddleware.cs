using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Library.SampleAuthentication.Extension
{
    /// <summary>
    /// 简易身份验证服务中间件
    /// </summary>
    /// <exception cref="SampleAuthenticationException"></exception>
    public class SampleAuthenticationMiddleware
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="options"></param>
        /// <param name="handler"></param>
        public SampleAuthenticationMiddleware(RequestDelegate next, CookieAuthenticationOptions options, IAuthenticationHandler handler)
        {
            Next = next;
            Options = options;
            Handler = handler;
        }

        #region 私有成员

        readonly PathString LoginPath;

        readonly PathString LogoutPath;

        readonly PathString AccessDeniedPath;

        readonly RequestDelegate Next;

        readonly CookieAuthenticationOptions Options;

        readonly IAuthenticationHandler Handler;

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
                if (context.Request.Method.Equals(HttpMethod.Post.Method)
                && context.Request.Path.HasValue)
                {
                    if (Options.LoginPath.Equals(context.Request.Path))
                    {
                        await Handler.Login(context).ConfigureAwait(false);
                        return;
                    }
                    else if (Options.LogoutPath.Equals(context.Request.Path))
                    {
                        await Handler.Logout(context).ConfigureAwait(false);
                        return;
                    }
                    else if (Options.AccessDeniedPath.Equals(context.Request.Path))
                    {
                        await Handler.AccessDenied(context).ConfigureAwait(false);
                        return;
                    }
                }

                await Next.Invoke(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new SampleAuthenticationException("中间件运行时发生异常.", ex);
            }
        }

        #endregion
    }
}
