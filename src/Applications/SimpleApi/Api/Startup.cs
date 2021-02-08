using Api.Configures;
using Api.Middleware;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using IocServiceDemo;
using Library.Cache;
using Library.Configuration;
using Library.Container;
using Library.TypeTool;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model.System.Config;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Reflection;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Config = new ConfigHelper(Configuration).GetModel<SystemConfig>("SystemConfig");
            Console.Title = Config.ProjectName;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// 系统配置
        /// </summary>
        public SystemConfig Config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region 注册服务示例代码

            //注册成单例
            //.AddSingleton(typeof(IDemoService),typeof(DemoServiceA))

            //使用工厂模式注册服务
            services.AddTransient(typeof(IDemoService), DemoServiceProvider.GetService);

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
            if (Config.RunMode != RunMode.Publish)
                SwaggerConfigura.RegisterServices(services, Config);

            if (Config.EnableCAS)
                CASConfigura.RegisterServices(services, Config);

            if (Config.EnableElasticsearch)
                ElasticsearchConfigura.RegisterServices(services, Config);

            if (Config.EnableFreeSql)
                FreeSqlConfigura.RegisterServices(services, Config);

            if (Config.EnableAutoMapper)
                AutoMapperConfigura.RegisterServices(services, Config);

            if (Config.EnableWeChatService)
                WeChatServiceConfigura.RegisterServices(services, Config);

            NLoggerConfigura.RegisterServices(services, Config);
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
            var diTypes = TypeHelper.GetTypes(Config.FxAssembly.ToArray())
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Request.Body重用
            app.Use(next => context =>
            {
                context.Request.EnableBuffering();

                return next(context);
            })
            .UseMiddleware<CorsMiddleware>()//跨域
            .UseDeveloperExceptionPage()
            .UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "application/octet-stream"
            })
            .UseRouting();

            //配置缓存
            CacheOption.Configure(option =>
            {
                option.CacheType = Config.DefaultCacheType;
                option.RedisConfig = Config.RedisConfig;
            });

            if (Config.RunMode != RunMode.Publish)
                SwaggerConfigura.RegisterApplication(app, Config);

            if (Config.EnableCAS)
                CASConfigura.RegisterApplication(app, Config);

            if (Config.EnableFreeSql)
                FreeSqlConfigura.RegisterApplication(app, Config);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            })
            //Nginx服务器
            .UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            //获取AutofacIOC容器
            AutofacHelper.Container = app.ApplicationServices.GetAutofacRoot();
        }
    }
}
