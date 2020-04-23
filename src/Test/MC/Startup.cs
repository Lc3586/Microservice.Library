using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Library.Extension;
using MC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;

namespace MC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            IdentityConfig._IdentityConfig = Configuration.GetSection("Identity").Get<Identity>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //关闭JWT Claim类型映射，以允许常用的Claim(sub、idp等等)
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = IdentityConfig._IdentityConfig.Scheme;
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect(IdentityConfig._IdentityConfig.Scheme, options =>
                {
                    if (!IdentityConfig._IdentityConfig.SignInScheme.IsNullOrEmpty())
                        options.SignInScheme = IdentityConfig._IdentityConfig.SignInScheme;
                    options.Authority = IdentityConfig._IdentityConfig.URI;
                    options.ClientId = IdentityConfig._IdentityConfig.ClientId;
                    if (!IdentityConfig._IdentityConfig.ClientSecret.IsNullOrEmpty())
                        options.ClientSecret = IdentityConfig._IdentityConfig.ClientSecret;

                    if (!IdentityConfig._IdentityConfig.ResponseType.IsNullOrEmpty())
                        options.ResponseType = IdentityConfig._IdentityConfig.ResponseType;

                    if (IdentityConfig._IdentityConfig.SaveTokens.HasValue)
                        options.SaveTokens = IdentityConfig._IdentityConfig.SaveTokens.Value;
                    options.RequireHttpsMetadata = IdentityConfig._IdentityConfig.useSSL;

                    var Scope = IdentityConfig._IdentityConfig.ScopeList;
                    if (Scope != null)
                    {
                        options.Scope.Clear();
                        Scope.ForEach(o => options.Scope.Add(o));
                    }

                    options.ClaimActions.DeleteClaim("name");
                    options.ClaimActions.DeleteClaim("given_name");
                    options.ClaimActions.DeleteClaim("family_name");
                    options.ClaimActions.DeleteClaim("email");
                    options.ClaimActions.DeleteClaim("email_verified");
                    options.ClaimActions.DeleteClaim("website");
                    options.ClaimActions.DeleteClaim("address");

                    options.Events = new OpenIdConnectEvents
                    {
                        OnRemoteFailure = (context) =>
                        {
                            context.Response.Redirect($"/Home/Error?Message={context.Failure.Message}");
                            context.HandleResponse();
                            return Task.FromResult(0);
                        }
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //显示隐藏信息（仅用于调试）
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //确保认证服务对每个请求都要验证
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
