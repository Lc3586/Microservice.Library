using Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Cache
{
    /// <summary>
    /// 缓存配置
    /// </summary>
    public class CacheOption
    {
        /// <summary>
        /// 配置
        /// </summary>
        internal static CacheOption Option = new CacheOption();

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="setupAction"></param>
        public static void Configure(Action<CacheOption> setupAction)
        {
            setupAction.Invoke(Option);
            CacheHelper.Init();
        }

        /// <summary>
        /// 缓存类型
        /// </summary>
        public CacheType CacheType { get; set; } = CacheType.SystemCache;
        /// <summary>
        /// Redis配置
        /// </summary>
        public string RedisConfig { get; set; }
    }
}
