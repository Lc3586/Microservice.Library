using Microservice.Library.Cache.Model;

namespace Microservice.Library.Cache.Application
{
    /// <summary>
    /// 缓存配置
    /// </summary>
    public class CacheGenOptions
    {
        /// <summary>
        /// 缓存类型
        /// </summary>
        /// <remarks>默认 <see cref="CacheType.SystemCache"/></remarks>
        public CacheType CacheType { get; set; } = CacheType.SystemCache;

        /// <summary>
        /// Redis配置
        /// </summary>
        public RedisOptions RedisOptions { get; set; } = new RedisOptions();
    }
}
