using FreeRedis;
using Library.Cache.Application;
using Library.Cache.Model;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Library.Cache.Services
{
    /// <summary>
    /// Redis缓存
    /// </summary>
    public class RedisCache : ICache
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options">配置</param>
        public RedisCache(RedisOptions options)
        {
            Options = options;
        }

        #region 私有成员

        readonly RedisOptions Options;

        void BuildRedisClient()
        {

            if (Options.ConnectionStrings != null && Options.ConnectionStrings.Any())
            {
                //读写分离
                if (Options.RW_Splitting)
                    Client = new RedisClient(
                        Options.ConnectionStrings.First(),
                        Options.ConnectionStrings.Skip(1)
                            .Select(o => ConnectionStringBuilder.Parse(o))
                            .ToArray());
                //集群
                else
                    Client = new RedisClient(Options.ConnectionStrings
                                                    .Select(o => ConnectionStringBuilder.Parse(o))
                                                    .ToArray());
            }
            else
            {
                //哨兵高可用
                if (Options.Sentinels != null && Options.Sentinels.Any())
                    Client = new RedisClient(
                        Options.ConnectionString,
                        Options.Sentinels.ToArray(),
                        Options.RW_Splitting);
                //普通
                else
                    Client = new RedisClient(Options.ConnectionString);
            }

            if (Options.ClientSideCachingOptions != null)
                Client.UseClientSideCaching(Options.ClientSideCachingOptions);

            if (Options.Subscribe != null && Options.Subscribe.Any())
                Client.Subscribe(Options.Subscribe.ToArray(), Options.ReceiveData);
        }

        /// <summary>
        /// 获取Redis客户端
        /// </summary>
        /// <returns></returns>
        RedisClient GetRedisClient()
        {
            if (Client == null)
                BuildRedisClient();

            return Client;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="expireType"></param>
        void SetCacheToRedis(string key, object value, TimeSpan? timeout, ExpireType? expireType)
        {
            string jsonStr;

            if (value is string)
                jsonStr = value as string;
            else
                jsonStr = JsonConvert.SerializeObject(value);

            RedisValueInfo valueInfo = new RedisValueInfo
            {
                Value = jsonStr,
                TypeName = value.GetType().FullName,
                ExpireTime = timeout,
                ExpireType = expireType
            };

            string theValue = JsonConvert.SerializeObject(valueInfo);

            if (timeout.HasValue)
                GetRedisClient().SetEx(key, timeout.Value.Seconds, theValue);
            else
                GetRedisClient().Set(key, theValue);
        }

        #endregion

        #region 公开接口

        /// <summary>
        /// Redis客户端
        /// </summary>
        public RedisClient Client { get; private set; }

        #region 设置缓存

        public void SetCache(string key, object value)
        {
            SetCacheToRedis(key, value, null, null);
        }

        public void SetCache(string key, object value, TimeSpan timeout)
        {
            SetCacheToRedis(key, value, timeout, ExpireType.Absolute);
        }

        public void SetCache(string key, object value, TimeSpan timeout, ExpireType expireType = ExpireType.Absolute)
        {
            SetCacheToRedis(key, value, timeout, expireType);
        }

        public void SetKeyExpire(string key, TimeSpan expire)
        {
            GetRedisClient().Expire(key, expire.Seconds);
        }

        #endregion

        #region 获取缓存

        public bool ContainsKey(string key)
        {
            return GetRedisClient().Exists(key);
        }

        public object GetCache(string key)
        {
            object value;

            var redisValue = GetRedisClient().Get(key);
            if (string.IsNullOrWhiteSpace(redisValue))
                return null;

            var valueInfo = JsonConvert.DeserializeObject<RedisValueInfo>(redisValue);

            if (valueInfo.TypeName == typeof(string).FullName)
                value = valueInfo.Value;
            else
                value = JsonConvert.DeserializeObject(valueInfo.Value, Type.GetType(valueInfo.TypeName));

            if (valueInfo.ExpireTime != null && valueInfo.ExpireType == ExpireType.Relative)
                SetKeyExpire(key, valueInfo.ExpireTime.Value);

            return value;
        }

        public T GetCache<T>(string key) where T : class
        {
            return (T)GetCache(key);
        }

        #endregion

        #region 移除缓存

        public void RemoveCache(string key)
        {
            GetRedisClient().Del(key);
        }

        #endregion

        #endregion
    }
}
