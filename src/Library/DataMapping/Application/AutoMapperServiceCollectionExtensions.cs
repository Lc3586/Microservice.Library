using AutoMapper;
using Library.DataMapping.Application;
using Library.DataMapping.Gen;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AutoMapperServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapper(
            this IServiceCollection services,
            Action<AutoMapperGenOptions> setupAction = null)
        {
            //注册自定义配置程序，将高级配置（<AutoMapperGen）应用于低级配置（AutoMapperGeneratorOptions）。
            services.AddTransient<IConfigureOptions<AutoMapperGeneratorOptions>, ConfigureAutoMapperGeneratorOptions>();

            //注册生成器和依赖
            services.AddTransient(s => s.GetRequiredService<IOptions<AutoMapperGenOptions>>().Value);

            services.AddSingleton<IAutoMapperProvider, AutoMapperGenerator>();

            if (setupAction != null) services.ConfigureFreeSql(setupAction);

            return services;
        }

        public static void ConfigureFreeSql(
            this IServiceCollection services,
            Action<AutoMapperGenOptions> setupAction)
        {
            services.Configure(setupAction);
        }
    }
}
