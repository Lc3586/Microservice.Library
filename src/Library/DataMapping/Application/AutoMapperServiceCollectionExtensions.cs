using Microservice.Library.DataMapping.Application;
using Microservice.Library.DataMapping.Gen;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class AutoMapperServiceCollectionExtensions
    {
        /// <summary>
        /// 注册数据映射组件
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoMapper(
            this IServiceCollection services,
            Action<AutoMapperGenOptions> setupAction = null)
        {
            //注册自定义配置程序，将高级配置（<AutoMapperGen）应用于低级配置（AutoMapperGeneratorOptions）。
            services.AddTransient<IConfigureOptions<AutoMapperGeneratorOptions>, ConfigureAutoMapperGeneratorOptions>();

            //注册生成器和依赖
            services.AddTransient(s => s.GetRequiredService<IOptions<AutoMapperGenOptions>>().Value);

            services.AddSingleton<IAutoMapperProvider, AutoMapperGenerator>();

            if (setupAction != null) services.ConfigureAutoMapper(setupAction);

            return services;
        }

        /// <summary>
        /// 配置数据映射组件
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        public static void ConfigureAutoMapper(
            this IServiceCollection services,
            Action<AutoMapperGenOptions> setupAction)
        {
            services.Configure(setupAction);
        }
    }
}
