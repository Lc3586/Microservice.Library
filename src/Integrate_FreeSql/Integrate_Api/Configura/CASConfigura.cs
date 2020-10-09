using AspNetCore.Security.CAS;
using Integrate_Business.Config;
using Integrate_Business.IAM;
using Integrate_Model.IAM;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Integrate_Api
{
    public class CASConfigura
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o =>
                {
                    o.LoginPath = new PathString("/casLogin");
                    o.AccessDeniedPath = new PathString("/casAccess-Denied");
                    o.Cookie = new CookieBuilder
                    {
                        Name = ".Cas"
                    };
                    o.Events = new CookieAuthenticationEvents
                    {
                        OnSigningIn = context =>
                        {
                            UserBusiness.RemoveCache(context.Principal.Claims?.FirstOrDefault(o => o.Type == "id").Value);
                            return Task.FromResult(0);
                        },
                        //OnSignedIn = context =>
                        //{
                        //    Console.WriteLine("已登录");
                        //    return Task.FromResult(0);
                        //},
                        //OnSigningOut = context =>
                        //{
                        //    Console.WriteLine("注销");
                        //    return Task.FromResult(0);
                        //},
                        //OnValidatePrincipal = context =>
                        //{
                        //    Console.WriteLine("校验票据");
                        //    return Task.FromResult(0);
                        //},
                        //OnRedirectToLogin = context =>
                        //{
                        //    Console.WriteLine("去登陆");
                        //    return Task.Run(() =>
                        //    {
                        //        Console.WriteLine("跳转登陆");
                        //        context.RedirectUri = o.LoginPath;
                        //    });
                        //},
                        //OnRedirectToLogout = context =>
                        //{
                        //    Console.WriteLine("去注销");
                        //    return Task.FromResult(0);
                        //},
                        //OnRedirectToAccessDenied = context =>
                        //{
                        //    Console.WriteLine("去验证");
                        //    return Task.FromResult(0);
                        //},
                        //OnRedirectToReturnUrl = context =>
                        //{
                        //    Console.WriteLine("返回");
                        //    return Task.FromResult(0);
                        //}
                    };
                })
                .AddCAS(o =>
                {
                    o.CasServerUrlBase = SystemConfig.systemConfig.CASBaseUrl;
                    o.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                });
            //services.AddCors(option =>
            //{
            //    option.AddPolicy("CASCors", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(SystemConfig.systemConfig.casCorsUrl));
            //});
            services.AddMvc();
        }

        /// <summary>
        /// 注：方法在UseMvc之前
        /// </summary>
        /// <param name="app"></param>
        public static void RegisterApplication(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseCors("CASCors");
            //app.Run(r =>
            //{
            //    r.Response.Redirect($"{new PathString("/casLogin")}?returnUrl={r.Request.Path}");
            //    return Task.FromResult(0);
            //});
        }
    }
}
