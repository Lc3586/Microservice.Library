using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IDASH.Models;
using IDASH.Validator;
using IdentityServer4;
using IdentityServer4.Quickstart.UI;
using Library.Extension;
using Microsoft.AspNetCore.Authentication.QQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IDASH
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 配置
        /// </summary>
        public IdentityConfig IdentityConfig { get; }

        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Environment = environment;
            Configuration = configuration;
            IdentityConfig = Configuration.GetSection("Identity").Get<IdentityConfig>();
            IdentityConfig.InitExAuthentication(Configuration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = services.AddIdentityServer()
                      //资源所有者用户名密码验证器
                      // .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                      .AddInMemoryIdentityResources(InMemoryConfiguration.GetIdentityResources())
                      .AddInMemoryApiResources(InMemoryConfiguration.GetApiResources())
                      .AddInMemoryClients(InMemoryConfiguration.GetClients())
                      .AddTestUsers(TestUsers.Users);

            if (Environment.IsDevelopment() || IdentityConfig.UseDevelopment == true)
            {
                //开发环境下使用临时证书
                builder.AddDeveloperSigningCredential(true, $"{Directory.GetCurrentDirectory()}/tempkey.rsa");
            }
            else
            {
                //正式环境下配置证书
                if (IdentityConfig.SigningCredential.IsNullOrEmpty())
                    throw new Exception("未配置安全证书!");
                var FileName = $"{Directory.GetCurrentDirectory()}/{IdentityConfig.SigningCredential}";
                if (!File.Exists(FileName))
                    throw new Exception("指定的安全证书不存在!");
                builder.AddSigningCredential(FileName);
            }

            #region 外部认证

            if (IdentityConfig.AnyExAuthentication)
            {
                var builder_Auth = services.AddAuthentication();
                if (IdentityConfig.QQ != null && IdentityConfig.QQ.Valid)
                    builder_Auth.AddQQ("QQ", options =>
                    {
                        options.AppId = IdentityConfig.QQ.AppId;
                        options.AppKey = IdentityConfig.QQ.AppKey;
                        if (IdentityConfig.QQ.SaveTokens.HasValue)
                            options.SaveTokens = IdentityConfig.QQ.SaveTokens.Value;
                    });

                if (IdentityConfig.Google != null && IdentityConfig.Google.Valid)
                    builder_Auth.AddGoogle("Google", options =>
                    {
                        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                        options.ClientId = IdentityConfig.Google.ClientId;
                        options.ClientSecret = IdentityConfig.Google.ClientSecret;
                        if (IdentityConfig.Google.SaveTokens.HasValue)
                            options.SaveTokens = IdentityConfig.Google.SaveTokens.Value;
                    });

                if (IdentityConfig.OpenIDConnect != null && IdentityConfig.OpenIDConnect.Valid)
                    builder_Auth.AddOpenIdConnect(IdentityConfig.OpenIDConnect.Scheme, IdentityConfig.OpenIDConnect.SchemeDisplayName, options =>
                    {
                        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                        options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                        if (IdentityConfig.OpenIDConnect.SaveTokens.HasValue)
                            options.SaveTokens = IdentityConfig.OpenIDConnect.SaveTokens.Value;

                        options.Authority = IdentityConfig.OpenIDConnect.Authority;
                        options.ClientId = IdentityConfig.OpenIDConnect.ClientId;
                        options.ClientSecret = IdentityConfig.OpenIDConnect.ClientSecret;

                        if (!IdentityConfig.OpenIDConnect.NameClaimType.IsNullOrEmpty() || !IdentityConfig.OpenIDConnect.RoleClaimType.IsNullOrEmpty())
                        {
                            options.TokenValidationParameters = new TokenValidationParameters();
                            if (!IdentityConfig.OpenIDConnect.NameClaimType.IsNullOrEmpty())
                                options.TokenValidationParameters.NameClaimType = IdentityConfig.OpenIDConnect.NameClaimType;
                            if (!IdentityConfig.OpenIDConnect.RoleClaimType.IsNullOrEmpty())
                                options.TokenValidationParameters.RoleClaimType = IdentityConfig.OpenIDConnect.RoleClaimType;
                        }
                    });
            }

            #endregion

            //初始化汉化配置
            SinicizationConfig.Init(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            //IdentityServer
            app.UseIdentityServer();

            app.UseHttpsRedirection();
            app.UseMvc();

            //Quickstart-UI
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
