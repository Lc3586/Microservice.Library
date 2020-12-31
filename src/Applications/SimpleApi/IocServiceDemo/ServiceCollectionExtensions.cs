using IocServiceDemo;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction">设置配置</param>
        /// <returns></returns>
        public static IServiceCollection AddDemoService(this IServiceCollection services, Action<DemoServiceOptions> setupAction = null)
        {
            //注册配置
            //services.AddTransient<IConfigureOptions<DemoServiceOptions>, ConfigureDemoServiceOptions>();
            services.AddTransient(s => s.GetRequiredService<IOptions<DemoServiceOptions>>().Value);

            //注册构造器
            services.AddTransient<IDemoServiceProvider, DemoServiceProvider>();

            //设置配置
            if (setupAction != null)
                services.Configure(setupAction);

            return services;
        }
    }
}
