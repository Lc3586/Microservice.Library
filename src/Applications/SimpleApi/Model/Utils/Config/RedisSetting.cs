using System.Collections.Generic;

namespace Model.Utils.Config
{
    /// <summary>
    /// Redis配置
    /// </summary>
    public class RedisSetting
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
        /// 本地缓存容量
        /// </summary>
        /// <remarks>服务端要求 6.0 及以上版本</remarks>
        public int ClientSideCachingCapacity { get; set; }

        /// <summary>
        /// 订阅集合
        /// </summary>
        public List<string> Subscribe { get; set; }
    }
}
