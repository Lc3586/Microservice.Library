using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Elasticsearch
{
    /// <summary>
    /// 生成选项
    /// </summary>
    public class ElasticsearchGeneratorOptions
    {
        public ElasticsearchGeneratorOptions()
        {
            ConnectionSettings = new ConnectionSettings();
            NumberOfShards = 5;
            NumberOfReplicas = 0;
        }

        /// <summary>
        /// 连接设置
        /// </summary>
        public ConnectionSettings ConnectionSettings { get; set; }

        /// <summary>
        /// 分片数量
        /// <para>默认5个</para>
        /// </summary>
        public int NumberOfShards { get; set; }

        /// <summary>
        /// 副本数量
        /// <para>默认0个</para>
        /// </summary>
        public int NumberOfReplicas { get; set; }
    }
}
