using IocServiceDemo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Config = Configuration.GetSection("SystemConfig").Get<SystemConfig>();
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
