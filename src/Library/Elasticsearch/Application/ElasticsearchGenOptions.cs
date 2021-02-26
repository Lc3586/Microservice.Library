using Elasticsearch.Net;
using Nest;
using System;

namespace Microservice.Library.Elasticsearch
{
    /// <summary>
    /// ES配置
    /// </summary>
    public class ElasticsearchGenOptions
    {
        /// <summary>
        /// 生成选项
        /// </summary>
        public ElasticsearchGeneratorOptions ElasticsearchGeneratorOptions { get; set; } = new ElasticsearchGeneratorOptions();
    }
}
