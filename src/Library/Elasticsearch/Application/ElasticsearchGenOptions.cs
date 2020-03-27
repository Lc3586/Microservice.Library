using Elasticsearch.Net;
using Nest;
using System;

namespace Library.Elasticsearch
{
    public class ElasticsearchGenOptions
    {
        public ElasticsearchGeneratorOptions ElasticsearchGeneratorOptions { get; set; } = new ElasticsearchGeneratorOptions();
    }
}
