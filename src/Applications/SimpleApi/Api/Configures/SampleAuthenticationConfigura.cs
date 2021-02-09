using Library.Container;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Model.System.Config;
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
                //options.Cookie.Name = ".SampleAuthentication";
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.LogoutPath = new PathString();

                options.SessionStore = AutofacHelper.GetScopeService<ITicketStore>();
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        Console.WriteLine("输出未登录提示.");
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.WriteAsync("未登录.");
                        return context.Response.CompleteAsync();
                    },
                    OnRedirectToAccessDenied = context =>
                    {
                        Console.WriteLine("输出禁止访问提示.");
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.WriteAsync("禁止访问.");
                        return context.Response.CompleteAsync();
                    }
                };
            })
            .AddSampleAuthentication(options =>
            {

            });

            return services;
        }

        /// <summary>
        /// 配置简易身份验证
        /// 注：方法在UseMvc之前
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        public static IApplicationBuilder ConfiguraSampleAuthentication(this IApplicationBuilder app, SystemConfig config)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
