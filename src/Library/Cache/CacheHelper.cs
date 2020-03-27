using Library.Models;
using System;

namespace Library.Cache
{
    /// <summary>
    /// 缓存帮助类
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 初始化
        /// </summary>
        internal static void Init()
        {
            _SystemCache = new SystemCache();

            if (!string.IsNullOrWhiteSpace(CacheOption.Option.RedisConfig))
            {
                try
                {
                    _RedisCache = new RedisCache(CacheOption.Option.RedisConfig);
                }
                catch
                {

                }
            }

            switch (CacheOption.Option.CacheType)
            {
                case CacheType.SystemCache: _Cache = new SystemCache(); break;
                case CacheType.RedisCache: _Cache = RedisCache; break;
                default: throw new Exception("请指定缓存类型！");
            }
        }

        /// <summary>
        /// 默认缓存
        /// </summary>
        private static ICache _Cache { get; set; }

        /// <summary>
        /// 默认缓存
        /// </summary>
        public static ICache Cache { get { return _Cache; } }

        /// <summary>
        /// 系统缓存
        /// </summary>
        private static ICache _SystemCache { get; set; }

        /// <summary>
        /// 系统缓存
        /// </summary>
        public static ICache SystemCache { get { return _SystemCache; } }

        /// <summary>
        /// Redis缓存
        /// </summary>
        private static ICache _RedisCache { get; set; }

        /// <summary>
        /// Redis缓存
        /// </summary>
        public static ICache RedisCache { get { return _RedisCache; } }
    }
}
