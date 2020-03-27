using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using SSH.Models;

namespace SSH
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            IdentityConfig = Configuration.GetSection("Identity").Get<IdentityConfig>();
            ServiceConfig = Configuration.GetSection("Service").Get<ServiceConfig>();
        }

        public IConfiguration Configuration { get; }
        public IdentityConfig IdentityConfig { get; }
        public ServiceConfig ServiceConfig { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                    .AddAuthorization()
                    .AddJsonFormatters();

            //IdentityServer
            services.AddAuthentication(IdentityConfig.Scheme)
            //旧
            //.AddIdentityServerAuthentication(option =>
            //{
            //    option.ApiName = ServiceConfig.Name;
            //    //option.ApiSecret = ServiceConfig.Secret;
            //    option.Authority = IdentityConfig.URI;
            //    option.RequireHttpsMetadata = IdentityConfig.useSSL;
            //});
            //新
            .AddJwtBearer(IdentityConfig.Scheme, options =>
            {
                options.Authority = IdentityConfig.URI;
                options.Audience = ServiceConfig.Name;
                options.RequireHttpsMetadata = IdentityConfig.useSSL;
            });

            if (ServiceConfig.Cors != null && ServiceConfig.Cors.Count > 0)
            {
                //允许跨域请求
                services.AddCors(options =>
                {
                    ServiceConfig.Cors.ForEach(cors =>
                    {
                        options.AddPolicy(cors.Name, policy =>
                        {
                            policy.WithOrigins(cors.Origins);
                            if (cors.AnyHeader == true)
                                policy.AllowAnyHeader();
                            if (cors.AnyMethod == true)
                                policy.AllowAnyMethod();
                        });
                    });
                });
            }
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (ServiceConfig.Cors != null && ServiceConfig.Cors.Count > 0)
            {
                //使用跨域配置
                ServiceConfig.Cors.ForEach(cors =>
                {
                    if (cors.Enable == true)
                        if (cors.Enable == true)
                            app.UseCors(cors.Name);
                });
            }

            //IdentityServer
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();

        }
    }
}
