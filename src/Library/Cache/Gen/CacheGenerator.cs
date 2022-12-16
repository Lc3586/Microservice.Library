using Microservice.Library.Cache.Application;
using Microservice.Library.Cache.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microservice.Library.Cache.Gen
{
    /// <summary>
    /// 微信服务生成器
    /// </summary>
    public class CacheGenerator : ICacheProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="serviceProvider"></param>
        public CacheGenerator(CacheGenOptions options, IServiceProvider serviceProvider)
        {
            Options = options ?? new CacheGenOptions();
            ServiceProvider = serviceProvider;
        }

        #region 私有成员

        /// <summary>
        /// 配置
        /// </summary>
        readonly CacheGenOptions Options;

        readonly IServiceProvider ServiceProvider;

        ICache Cache;

        #endregion

        #region 公共方法

        public ICache GetCache()
        {
            if (Cache == null)
            {
                switch (Options.CacheType)
                {
                    case Model.CacheType.RedisCache:
                        if (Options.RedisOptions == null)
                            throw new CacheException($"{nameof(Options.RedisOptions)}配置有误.");

                        Cache = new RedisCache(Options.RedisOptions);
                        break;
                    case Model.CacheType.SystemCache:
                    default:
                        var memoryCache = ServiceProvider.GetService<IMemoryCache>();
                        if (memoryCache != null)
                            Cache = new SystemCache(memoryCache);
                        else
                            Cache = new SystemCache();
                        break;
                }
            }

            return Cache;
        }

        public RedisCache GetRedis()
        {
            if (Options.CacheType == Model.CacheType.RedisCache)
                return (RedisCache)GetCache();

            if (Options.RedisOptions == null)
                throw new CacheException($"{nameof(Options.RedisOptions)}配置有误.");

            return new RedisCache(Options.RedisOptions);
        }

        #endregion
    }
}
