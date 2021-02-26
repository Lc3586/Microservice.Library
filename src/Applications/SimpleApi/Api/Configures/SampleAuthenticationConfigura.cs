using Microservice.Library.Container;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Model.Utils.Config;
using System;

namespace Api.Configures
{
    /// <summary>
    /// 简易身份验证配置类
    /// </summary>
    public static class SampleAuthenticationConfigura
    {
        /// <summary>
        /// 注册简易身份验证服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static IServiceCollection RegisterSampleAuthentication(this IServiceCollection services, SystemConfig config)
        {
            services.AddControllers(options =>
            {
                // Global Authorize Filter
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = ".SA";
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);

                options.Cookie.SameSite = SameSiteMode.Strict;

                options.LoginPath = new PathString("/sa/login");
                options.LogoutPath = new PathString("/sa/logout");
                options.AccessDeniedPath = new PathString("/sa/access-denied");

                options.SessionStore = AutofacHelper.GetScopeService<ITicketStore>();
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
#if DEBUG
                        Console.WriteLine("输出未登录提示.");
#endif
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "text/plain;charset=UTF-8";
                        context.Response.WriteAsync("未登录.");
                        return context.Response.CompleteAsync();
                    },
                    //OnRedirectToAccessDenied = context =>
                    //{
//#if DEBUG
//                    Console.WriteLine("输出禁止访问提示.");
//#endif
                //    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                //    context.Response.ContentType = "text/plain;charset=UTF-8";
                //    context.Response.WriteAsync("拒绝访问.");
                //    return context.Response.CompleteAsync();
                //}
            };
            });

            return services;
        }

        /// <summary>
        /// 配置简易身份验证
        /// 注：方法在UseEndpoints之前调用
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        public static IApplicationBuilder ConfiguraSampleAuthentication(this IApplicationBuilder app, SystemConfig config)
        {
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
