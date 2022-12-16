using Microservice.Library.Cache.Application;
using Microservice.Library.Cache.Gen;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入扩展方法
    /// </summary>
    public static class CacheServiceCollectionExtensions
    {
        /// <summary>
        /// 添加缓存
        /// <para>单例模式</para>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddCache(
            this IServiceCollection services,
            Action<CacheGenOptions> setupAction = null)
        {
            //注册自定义配置程序，将高级配置（CacheGenOptions）应用于低级配置（RedisOptions）。
            services.AddTransient<IConfigureOptions<RedisOptions>, ConfigureRedisOptions>();

            //注册生成器和依赖
            services.AddTransient(s => s.GetRequiredService<IOptions<CacheGenOptions>>().Value);

            services.AddSingleton<ICacheProvider, CacheGenerator>();

            if (setupAction != null) services.ConfigureCache(setupAction);

            return services;
        }

        /// <summary>
        /// 配置缓存
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        public static void ConfigureCache(
            this IServiceCollection services,
            Action<CacheGenOptions> setupAction)
        {
            services.Configure(setupAction);
        }
    }
}
