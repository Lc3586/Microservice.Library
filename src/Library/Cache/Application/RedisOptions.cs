using FreeRedis;
using System;
using System.Collections.Generic;

namespace Microservice.Library.Cache.Application
{
    /// <summary>
    /// Redis配置
    /// </summary>
    public class RedisOptions
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <remarks>使用读写分离、集群等功能时请配置<see cref="ConnectionStrings"/>选项</remarks>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <remarks>
        /// <para>使用读写分离、集群等功能时请配置此选项</para>
        /// <para>集合中的第一个连接将作为写入连接</para>
        /// </remarks>
        public List<string> ConnectionStrings { get; set; }

        /// <summary>
        /// 哨兵
        /// </summary>
        public List<string> Sentinels { get; set; }

        /// <summary>
        /// 是否设置读写分离
        /// </summary>
        public bool RW_Splitting { get; set; } = true;

        /// <summary>
        /// 本地缓存配置
        /// </summary>
        /// <remarks>服务端要求 6.0 及以上版本</remarks>
        public ClientSideCachingOptions ClientSideCachingOptions { get; set; }

        /// <summary>
        /// 订阅集合
        /// </summary>
        public List<string> Subscribe { get; set; }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <remarks>
        /// <para>参数1: 订阅</para>
        /// <para>参数2: 数据</para>
        /// </remarks>
        public Action<string, object> ReceiveData;
    }
}
