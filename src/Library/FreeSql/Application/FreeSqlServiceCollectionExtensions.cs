using Microservice.Library.FreeSql.Application;
using Microservice.Library.FreeSql.Gen;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// FreeSql依赖注入扩展类
    /// </summary>
    public static class FreeSqlServiceCollectionExtensions
    {
        /// <summary>
        /// 注册单库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddFreeSql(
            this IServiceCollection services,
            Action<FreeSqlGenOptions> setupAction = null)
        {
            //注册自定义配置程序，将高级配置（<FreeSqlGen）应用于低级配置（FreeSqlGeneratorOptions,FreeSqlDevOptions,FreeSqlDbContextOptions）。
            services.AddTransient<IConfigureOptions<FreeSqlGeneratorOptions>, ConfigureFreeSqlGeneratorOptions>();
            services.AddTransient<IConfigureOptions<FreeSqlDevOptions>, ConfigureFreeSqlDevOptions>();
            services.AddTransient<IConfigureOptions<FreeSqlDbContextOptions>, ConfigureFreeSqlDbContextOptions>();
            services.AddTransient(s => s.GetRequiredService<IOptions<FreeSqlGenOptions>>().Value);

            if (setupAction != null) services.ConfigureFreeSql(setupAction);

            //注册生成器和依赖
            services.AddSingleton<IFreeSqlProvider, FreeSqlGenerator>();

            return services;
        }

        /// <summary>
        /// 注册多库
        /// </summary>
        /// <typeparam name="TKey">库标识类型</typeparam>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddFreeSql<TKey>(
            this IServiceCollection services,
            Action<FreeSqlMultipleGenOptions<TKey>> setupAction = null)
        {
            //注册配置
            services.AddTransient(s => s.GetRequiredService<IOptions<FreeSqlMultipleGenOptions<TKey>>>().Value);

            if (setupAction != null) services.ConfigureFreeSql(setupAction);

            //注册生成器和依赖
            services.AddSingleton<IFreeSqlMultipleProvider<TKey>, FreeSqlMultipleGenerator<TKey>>();

            return services;
        }

        /// <summary>
        /// 配置单库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        public static void ConfigureFreeSql(
            this IServiceCollection services,
            Action<FreeSqlGenOptions> setupAction)
        {
            services.Configure(setupAction);
        }

        /// <summary>
        /// 配置多库
        /// </summary>
        /// <typeparam name="TKey">库标识类型</typeparam>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        public static void ConfigureFreeSql<TKey>(
            this IServiceCollection services,
            Action<FreeSqlMultipleGenOptions<TKey>> setupAction)
        {
            services.Configure(setupAction);
        }
    }
}
