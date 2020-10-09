using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Library.Container;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Library.Models;
using Library.DataMapping;
using Library.Configuration;
using Library.Extension;
using Coldairarrow.Util;
using Library.DataAccess;
using Library.Elasticsearch;
using Library.Cache;
using Integrate_Business.Config;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Castle.Core.Logging;
using AutoMapper;
using Integrate_Api.Configura;

namespace Integrate_Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            SystemConfig.systemConfig = Configuration.GetSection("SystemConfig").Get<SystemConfig>();
            Console.Title = SystemConfig.systemConfig.ProjectName;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            })
            .AddNewtonsoftJson(options =>
            {
                //����ȫ������
                //options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            })//��������Ϊapplication/jsonʱ��������Ӵ˷���
            .AddControllersAsServices();

            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>()
            .AddTransient<IActionContextAccessor, ActionContextAccessor>()
            .AddSingleton(Configuration)
            .AddLogging()
            .Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            })
            .Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            if (SystemConfig.systemConfig.RunMode != RunMode.Publish)
                SwaggerConfigura.RegisterServices(services);

            if (SystemConfig.systemConfig.CASEnable)
                CASConfigura.RegisterServices(services);

            if (SystemConfig.systemConfig.EnableElasticsearch)
                ElasticsearchConfigura.RegisterServices(services);

            if (SystemConfig.systemConfig.EnableFreeSql)
                FreeSqlConfigura.RegisterServices(services);

            if (SystemConfig.systemConfig.EnableAutoMapper)
                AutoMapperConfigura.RegisterServices(services);

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // ��������ӷ���ע��
            var baseType = typeof(IDependency);

            //�Զ�ע��IDependency�ӿ�,֧��AOP,��������ΪInstancePerDependency
            var diTypes = SystemConfig.systemConfig.FxTypes
                .Where(x => baseType.IsAssignableFrom(x) && x != baseType)
                .ToArray();
            builder.RegisterTypes(diTypes)
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .InstancePerDependency()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(Interceptor));

            //ע��Controller
            builder.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly)
                .Where(t => typeof(Controller).IsAssignableFrom(t) && t.Name.EndsWith("Controller", StringComparison.Ordinal))
                .PropertiesAutowired();

            //AOP
            builder.RegisterType<Interceptor>();

            //��������Զ��ͷ�
            builder.RegisterType<DisposableContainer>()
                .As<IDisposableContainer>()
                .InstancePerLifetimeScope();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env/*, IApiVersionDescriptionProvider provider*/)
        {
            //Request.Body����
            app.Use(next => context =>
            {
                context.Request.EnableBuffering();

                return next(context);
            })
            .UseMiddleware<CorsMiddleware>()//����
            //.UseMiddleware<CheckStaticFilePermissionMiddleware>()//��̬�ļ���Ȩ
            .UseDeveloperExceptionPage()
            .UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "application/octet-stream"
            })
            .UseRouting();


            if (SystemConfig.systemConfig.RunMode != RunMode.Publish)
                SwaggerConfigura.RegisterApplication(app/*, provider*/);

            if (SystemConfig.systemConfig.CASEnable)
                CASConfigura.RegisterApplication(app);

            if (SystemConfig.systemConfig.EnableFreeSql)
                FreeSqlConfigura.RegisterApplication(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            })
            //Nginx������
            .UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            //DbOption.Configure(opetion =>
            //{
            //    opetion.ConnectionString = SystemConfig.systemConfig.DefaultDatabaseConnectString;
            //    opetion.DbType = SystemConfig.systemConfig.DefaultDatabaseType;
            //    opetion.EntityAssembly = "Integrate_Entity";
            //});

            CacheOption.Configure(option =>
            {
                option.CacheType = SystemConfig.systemConfig.DefaultCacheType;
                option.RedisConfig = SystemConfig.systemConfig.RedisConfig;
            });

            AutofacHelper.Container = app.ApplicationServices.GetAutofacRoot();

            InitId();
        }

        private void InitId()
        {
            new IdHelperBootstrapper()
                //����WorkerId
                .SetWorkderId(SystemConfig.systemConfig.WorkerId)
                //ʹ��Zookeeper
                //.UseZookeeper("127.0.0.1:2181", 200, SystemConfig.systemConfig.ProjectName)
                .Boot();
        }
    }
}
