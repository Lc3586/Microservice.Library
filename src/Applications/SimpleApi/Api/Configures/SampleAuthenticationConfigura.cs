using Library.Container;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Model.System.Config;
using System;
using System.Linq;
using System.Threading.Tasks;

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
                //options.Cookie.Name = ".Cas";
                options.LoginPath = new PathString("/casLogin");
                options.AccessDeniedPath = new PathString("/casAccess-Denied");
                options.LogoutPath = new PathString("/casLogout");
                options.Cookie.SameSite = SameSiteMode.Strict;

                options.SessionStore = AutofacHelper.GetScopeService<ITicketStore>();
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        if (context.Request.Path.Value.Equals("/casLogin", StringComparison.OrdinalIgnoreCase)
                            || (context.Request.Path.Value.Equals("/casAuthorize", StringComparison.OrdinalIgnoreCase)
                                && context.Request.Method.Equals(System.Net.Http.HttpMethod.Get.Method, StringComparison.OrdinalIgnoreCase)))
                        {
                            context.Response.Redirect($"{options.LoginPath}?returnUrl={context.Request.Path.Value}{context.Request.QueryString}");
                            return Task.CompletedTask;
                        }
                        else
                        {
                            Console.WriteLine("输出未登录提示");
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.WriteAsync("未登录");
                            return context.Response.CompleteAsync();
                        }
                    },
                    //OnSignedIn = context =>
                    //{
                    //    var id = context.Principal.Claims?.FirstOrDefault(t => t.Type == "id").Value;
                    //    UserBusiness.RemoveCache(id);
                    //    context.Response.Cookies.Append(".Cas", context.Request.Cookies[".AspNetCore.Cookies"]);
                    //    return Task.CompletedTask;
                    //},
                    OnSigningOut = context =>
                    {
                        if (context.HttpContext.User.Identity.IsAuthenticated)
                        {
                            var id = context.HttpContext.User.Claims?.FirstOrDefault(t => t.Type == "id").Value;

                        }

                        // Single Sign-Out
                        var casUrl = new Uri(config.CASBaseUrl);
                        var links = context.HttpContext.RequestServices.GetRequiredService<LinkGenerator>();
                        //var serviceUrl = links.GetUriByPage(context.HttpContext, "");
                        var redirectUri = UriHelper.BuildAbsolute(
                            casUrl.Scheme,
                            new HostString(casUrl.Host, casUrl.Port),
                            casUrl.LocalPath, "/logout",
                            QueryString.Create("service", config.WebRootUrl));

                        var logoutRedirectContext = new RedirectContext<CookieAuthenticationOptions>(
                            context.HttpContext,
                            context.Scheme,
                            context.Options,
                            context.Properties,
                            redirectUri
                        );
                        context.Response.StatusCode = 204; //Prevent RedirectToReturnUrl
                        context.Options.Events.RedirectToLogout(logoutRedirectContext);
                        return Task.CompletedTask;
                    }
                };
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
