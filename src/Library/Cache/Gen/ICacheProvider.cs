using Microservice.Library.Cache.Services;

namespace Microservice.Library.Cache.Gen
{
    /// <summary>
    /// 缓存构造器
    /// </summary>
    /// <exception cref="Application.CacheException"></exception>
    public interface ICacheProvider
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <returns></returns>
        ICache GetCache();

        /// <summary>
        /// 获取Redis
        /// </summary>
        /// <returns></returns>
        RedisCache GetRedis();
    }
}
