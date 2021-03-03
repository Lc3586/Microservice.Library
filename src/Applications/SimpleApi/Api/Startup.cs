using Api.Configures;
using Api.Middleware;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using IocServiceDemo;
using Microservice.Library.Configuration;
using Microservice.Library.Container;
using Microservice.Library.Extension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model.Utils.Config;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Reflection;

namespace Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Config = new ConfigHelper(Configuration).GetModel<SystemConfig>("SystemConfig");
            Console.Title = Config.ProjectName;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 系统配置
        /// </summary>
        public SystemConfig Config { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <remarks>
        /// <para>This method gets called by the runtime. Use this method to add services to the container.</para>
        /// <para>For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940</para>
        /// </remarks>
        public void ConfigureServices(IServiceCollection services)
        {
            #region 注册服务示例代码

            //注册成单例
            //.AddSingleton(typeof(IDemoService),typeof(DemoServiceA))

            //使用工厂模式注册服务
            services.AddScoped(typeof(IDemoService), DemoServiceProvider.GetService);

            //通过自定义方法注册服务构造器
            services.AddDemoService(options =>
            {
                options.Threshold = 2;
                //options.DisableType.Add(typeof(DemoServiceC));
            });

            #endregion

            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            })
            .AddNewtonsoftJson(options =>
            {
                //可在此配置Json序列化全局设置
                //options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            })
            .AddControllersAsServices();

            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>()
            .AddTransient<IActionContextAccessor, ActionContextAccessor>()
            .AddSingleton(Configuration)
            .AddSingleton(Config)
            //启用自带的日志组件
            .AddLogging()
            .Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            })
            .Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            //不是发布模式时，开放swagger接口文档
            if (Config.EnableSwagger && Config.RunMode != RunMode.Publish)
            {
                if (Config.Swagger.EnableApiMultiVersion)
                    services.RegisterSwaggerMultiVersion(Config);
                else
                    services.RegisterSwagger(Config);
            }

            if (Config.EnableCache)
                services.RegisterCache(Config);

            if (Config.EnableSampleAuthentication)
                services.RegisterSampleAuthentication(Config);

            if (Config.EnableCAS)
                services.RegisterCAS(Config);

            if (Config.EnableElasticsearch)
                services.RegisterElasticsearch(Config);

            if (Config.EnableKafka)
                services.RegisterKafka(Config);

            if (Config.EnableFreeSql)
            {
                if (Config.EnableMultiDatabases)
                    services.RegisterFreeSqlMultiDatabase(Config);
                else
                    services.RegisterFreeSql(Config);
            }

            if (Config.EnableAutoMapper)
                services.RegisterAutoMapper(Config);

            if (Config.EnableWeChatService)
                services.RegisterWeChat(Config);

            if (Config.EnableSoap)
                services.RegisterSoap(Config);

            services.RegisterNLog(Config);
        }

        /// <summary>
        /// 配置Autofac容器
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // 在这里添加服务注册
            var baseType = typeof(IDependency);

            //自动注入IDependency接口,支持AOP,生命周期为InstancePerDependency
            var diTypes = Config.FxAssembly.GetTypes()
                .Where(x => baseType.IsAssignableFrom(x) && x != baseType)
                .ToArray();
            builder.RegisterTypes(diTypes)
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .InstancePerDependency()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(Interceptor));

            //注册Controller
            builder.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly)
                .Where(t => typeof(Controller).IsAssignableFrom(t) && t.Name.EndsWith(nameof(Controller), StringComparison.Ordinal))
                .PropertiesAutowired();

            //AOP
            builder.RegisterType<Interceptor>();

            //请求结束自动释放
            builder.RegisterType<DisposableContainer>()
                .As<IDisposableContainer>()
                .InstancePerLifetimeScope();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <remarks>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </remarks>
#pragma warning disable IDE0060 // 删除未使用的参数
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#pragma warning restore IDE0060 // 删除未使用的参数
        {
            //Request.Body重用
            app.Use(next => context =>
            {
                context.Request.EnableBuffering();

                return next(context);
            })
            //处理异常
            .UseMiddleware<ExceptionHandlerMiddleware>()
            //跨域
            .UseMiddleware<CorsMiddleware>()
            //.UseDeveloperExceptionPage()
            .UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "application/octet-stream"
            })
            .UseRouting();

            //不是发布模式时，开放swagger接口文档
            if (Config.EnableSwagger && Config.RunMode != RunMode.Publish)
            {
                if (Config.Swagger.EnableApiMultiVersion)
                    app.ConfiguraSwaggerMultiVersion(Config);
                else
                    app.ConfiguraSwagger(Config);
            }

            if (Config.EnableSampleAuthentication)
                app.ConfiguraSampleAuthentication(Config);

            if (Config.EnableCAS)
                app.ConfiguraCAS(Config);

            if (Config.EnableFreeSql)
                app.ConfiguraFreeSql(Config);

            if (Config.EnableFreeSql)
            {
                if (Config.EnableMultiDatabases)
                    app.ConfiguraFreeSqlMultiDatabase(Config);
                else
                    app.ConfiguraFreeSql(Config);
            }

            if (Config.EnableWeChatService)
                app.ConfiguraWeChat(Config);

            if (Config.EnableKafka)
                app.ConfiguraKafka(Config);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            })
            //Nginx服务器
            .UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (Config.EnableSoap)
                app.ConfiguraSoap(Config);

            //获取AutofacIOC容器
            AutofacHelper.Container = app.ApplicationServices.GetAutofacRoot();
        }
    }
}
