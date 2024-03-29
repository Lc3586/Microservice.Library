﻿using Microservice.Library.Extension;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.Elasticsearch
{
    internal class ConfigureElasticsearchGeneratorOptions : IConfigureOptions<ElasticsearchGeneratorOptions>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ElasticsearchGenOptions _elasticsearchGenOptions;

        public ConfigureElasticsearchGeneratorOptions(
            IServiceProvider serviceProvider,
            IOptions<ElasticsearchGenOptions> elasticsearchGenOptionsAccessor)
        {
            _serviceProvider = serviceProvider;
            _elasticsearchGenOptions = elasticsearchGenOptionsAccessor.Value;
        }

        public void Configure(ElasticsearchGeneratorOptions options)
        {
            DeepCopy(_elasticsearchGenOptions.ElasticsearchGeneratorOptions, options);
        }

        private void DeepCopy(ElasticsearchGeneratorOptions source, ElasticsearchGeneratorOptions target)
        {
            target.ConnectionSettings = source.ConnectionSettings;
            target.NumberOfShards = source.NumberOfShards;
            target.NumberOfReplicas = source.NumberOfReplicas;
        }
    }
}
