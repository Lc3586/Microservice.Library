using Api.Middleware;
using GSS.Authentication.CAS;
using GSS.Authentication.CAS.AspNetCore;
using GSS.Authentication.CAS.Validation;
using Library.Container;
using Library.NLogger.Gen;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Model.System.Config;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Configures
{
    /// <summary>
    /// CAS配置类
    /// </summary>
    public static class CASConfigura
    {
        /// <summary>
        /// 注册CAS服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static IServiceCollection RegisterCAS(this IServiceCollection services, SystemConfig config)
        {
            services.AddControllers(options =>
            {
                // Global Authorize Filter
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddDistributedMemoryCache();

            services.AddSingleton<IServiceTicketStore, DistributedCacheServiceTicketStore>();
            services.AddSingleton<ITicketStore, TicketStoreWrapper>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                //options.Cookie.Name = ".Cas";
                options.LoginPath = new PathString("/casLogin");
                options.AccessDeniedPath = new PathString("/casAccess-Denied");
                options.LogoutPath = new PathString("/casLogout");
                options.Cookie.SameSite = SameSiteMode.None;

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
            })
            .AddCAS(options =>
            {
                options.CasServerUrlBase = config.CASBaseUrl;
                options.AccessDeniedPath = new PathString("/casAccess-Denied");
                // required for CasSingleSignOutMiddleware
                options.SaveTokens = true;
                var protocolVersion = config.CASProtocolVersion;
                if (protocolVersion != "3")
                {
                    switch (protocolVersion)
                    {
                        case "1":
                            options.ServiceTicketValidator = new Cas10ServiceTicketValidator(options);
                            break;
                        case "2":
                            options.ServiceTicketValidator = new Cas20ServiceTicketValidator(options);
                            break;
                        case "custom":
                            options.ServiceTicketValidator = new CasCustomServiceTicketValidator(options);
                            break;
                    }
                }
                options.Events = new CasEvents
                {
                    OnCreatingTicket = context =>
                    {
                        var assertion = context.Assertion;
                        if (assertion == null)
                            return Task.CompletedTask;
                        if (context.Principal.Identity is not ClaimsIdentity identity)
                            return Task.CompletedTask;
                        // Map claims from assertion
                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, assertion.PrincipalName));

                        if (assertion.Attributes != null)
                        {
                            foreach (var attributes in assertion.Attributes)
                            {
                                identity.AddClaim(new Claim(attributes.Key, attributes.Value));
                            }
                        }
                        return Task.CompletedTask;
                    },
                    OnRemoteFailure = context =>
                    {
                        var failure = context.Failure;

                        var logger = AutofacHelper.GetScopeService<INLoggerProvider>()
                                                .GetNLogger(config.DefaultLoggerName);

                        logger.Error(failure, failure.Message);

                        context.Response.Redirect("/casExternalLoginFailure");
                        context.HandleResponse();

                        return Task.CompletedTask;
                    }
                };
            });

            //services.AddMvc();

            return services;
        }

        /// <summary>
        /// 配置CAS
        /// 注：方法在UseMvc之前
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        public static IApplicationBuilder ConfiguraCAS(this IApplicationBuilder app, SystemConfig config)
        {
            //启用单点注销
            if (config.EnableCasSingleSignOut)
                app.UseMiddleware<Middleware.CasSingleSignOutMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
