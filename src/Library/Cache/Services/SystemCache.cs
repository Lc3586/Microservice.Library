using Microservice.Library.Cache.Extention;
using Microservice.Library.Cache.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microservice.Library.Cache.Services
{
    /// <summary>
    /// 系统缓存
    /// </summary>
    public class SystemCache : ICache
    {
        /// <summary>
        /// 
        /// </summary>
        public SystemCache()
        {
            MemoryCache = new ServiceCollection()
                .AddMemoryCache()
                .BuildServiceProvider()
                .GetService<IMemoryCache>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryCache"></param>
        public SystemCache(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }

        #region 私有成员

        readonly IMemoryCache MemoryCache;

        #endregion

        #region 公开接口

        #region 设置缓存

        public void SetCache(string key, object value)
        {
            MemoryCache.Set(key, value);
        }

        public void SetCache(string key, object value, TimeSpan timeout)
        {
            MemoryCache.Set(key, value, new DateTimeOffset(DateTimeExtention.GetCstTime() + timeout));
        }

        public void SetCache(string key, object value, TimeSpan timeout, ExpireType expireType = ExpireType.Absolute)
        {
            if (expireType == ExpireType.Absolute)
                MemoryCache.Set(key, value, new DateTimeOffset(DateTimeExtention.GetCstTime() + timeout));
            else
                MemoryCache.Set(key, value, timeout);
        }

        public void SetKeyExpire(string key, TimeSpan expire)
        {
            var value = GetCache(key);
            SetCache(key, value, expire);
        }

        #endregion

        #region 获取缓存

        public bool ContainsKey(string key)
        {
            return MemoryCache.TryGetValue(key, out object value);
        }

        public object GetCache(string key)
        {
            return MemoryCache.Get(key);
        }

        public T GetCache<T>(string key) where T : class
        {
            return (T)GetCache(key);
        }

        #endregion

        #region 移除缓存

        public void RemoveCache(string key)
        {
            MemoryCache.Remove(key);
        }

        #endregion

        #endregion
    }
}