using Confluent.Kafka;
using System.Collections.Generic;

namespace Model.Utils.Config
{
    /// <summary>
    /// kafka中间件配置
    /// </summary>
    public class KafkaSetting
    {
        #region 生产者

        /// <summary>
        /// 启用生产者
        /// </summary>
        public bool EnableProducer { get; set; }

        /// <summary>
        /// 生产者组标识
        /// </summary>
        public string ProducerGroupId { get; set; }

        /// <summary>
        /// 生产者服务器
        /// </summary>
        public string ProducerServers { get; set; }

        /// <summary>
        /// 生产者自动偏移复位
        /// </summary>
        public AutoOffsetReset? ProducerReset { get; set; }

        /// <summary>
        /// 生产者订阅集合
        /// </summary>
        public List<string> ProducerTopics { get; set; }

        #endregion

        #region 消费者

        /// <summary>
        /// 启用消费者
        /// </summary>
        public bool EnableConsume { get; set; }

        /// <summary>
        /// 消费者组标识
        /// </summary>
        public string ConsumeGroupId { get; set; }

        /// <summary>
        /// 消费者服务器
        /// </summary>
        public string ConsumeServers { get; set; }

        /// <summary>
        /// 消费者自动偏移复位
        /// </summary>
        public AutoOffsetReset? ConsumeReset { get; set; }

        /// <summary>
        /// 消费者订阅集合
        /// </summary>
        public List<string> ConsumeTopics { get; set; }

        #endregion
    }
}
