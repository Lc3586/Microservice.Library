using Business.Utils.Log;
using Microservice.Library.Extension;
using Microsoft.Extensions.DependencyInjection;
using Model.Utils.Config;
using Model.Utils.Log;

namespace Api.Configures
{
    /// <summary>
    /// 缓存配置类
    /// </summary>
    public static class CacheConfigura
    {
        /// <summary>
        /// 注册缓存
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static IServiceCollection RegisterCache(this IServiceCollection services, SystemConfig config)
        {
            services.AddCache(options =>
            {
                options.CacheType = config.DefaultCacheType;
                options.RedisOptions = new Microservice.Library.Cache.Application.RedisOptions
                {
                    ConnectionString = config.Redis.ConnectionString,
                    ConnectionStrings = config.Redis.ConnectionStrings,
                    Sentinels = config.Redis.Sentinels,
                    RW_Splitting = config.Redis.RW_Splitting,
                    ClientSideCachingOptions = new FreeRedis.ClientSideCachingOptions
                    {
                        Capacity = config.Redis.ClientSideCachingCapacity
                    },
                    Subscribe = config.Redis.Subscribe,
                    ReceiveData = (channel, data) =>
                    {
                        Logger.Log(
                            NLog.LogLevel.Trace,
                            LogType.系统跟踪,
                            $"redis channel: {channel}, receive data.",
                            data.ToJson());
                    }
                };
            });

            if (config.DefaultCacheType == Microservice.Library.Cache.Model.CacheType.SystemCache)
                services.AddMemoryCache();

            return services;
        }
    }
}
