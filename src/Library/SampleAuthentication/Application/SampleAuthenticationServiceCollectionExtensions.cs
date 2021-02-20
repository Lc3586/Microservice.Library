using Library.SampleAuthentication.Extension;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 简易身份验证服务依赖注入扩展类
    /// </summary>
    public static class SampleAuthenticationServiceCollectionExtensions
    {
        /// <summary>
        /// 注册简易身份验证服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddSampleAuthentication(
            this IServiceCollection services,
            Action<CookieAuthenticationOptions> configureOptions = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            return services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(configureOptions);
        }

        /// <summary>
        /// 配置简易身份验证服务中间件
        /// </summary>
        /// <param name="app"></param>
        /// <exception cref="SampleAuthenticationException"></exception>
        /// <returns></returns>
        public static IApplicationBuilder UseSampleAuthentication(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            return app.UseMiddleware<SampleAuthenticationMiddleware>();
        }
    }
}
