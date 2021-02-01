using Library.NLogger.Application;
using Library.NLogger.Gen;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// NLogger依赖注入扩展类
    /// </summary>
    public static class NLoggerServiceCollectionExtensions
    {
        /// <summary>
        /// 注册日志组件
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddNLogger(
            this IServiceCollection services,
            Action<NLoggerGenOptions> setupAction = null)
        {
            //注册自定义配置程序
            services.AddTransient(s => s.GetRequiredService<IOptions<NLoggerGenOptions>>().Value);

            if (setupAction != null) services.ConfigureNLogger(setupAction);

            //注册生成器和依赖
            services.AddSingleton<INLoggerProvider, NLoggerGenerator>();

            return services;
        }

        /// <summary>
        /// 配置日志组件
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        public static void ConfigureNLogger(
            this IServiceCollection services,
            Action<NLoggerGenOptions> setupAction)
        {
            services.Configure(setupAction);
        }
    }
}
