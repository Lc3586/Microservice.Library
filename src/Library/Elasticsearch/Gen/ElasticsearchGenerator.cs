using Library.Elasticsearch.Annotations;
using Library.Extention;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Elasticsearch.Gen
{
    public class ElasticsearchGenerator : IElasticsearchProvider
    {
        private readonly ElasticsearchGeneratorOptions _options;

        public ElasticsearchGenerator(ElasticsearchGeneratorOptions options)
        {
            _options = options ?? new ElasticsearchGeneratorOptions();
        }

        public ElasticsearchClient GetElasticsearch<T>(DateTime? state = null) where T : class
        {
            var type = typeof(T);
            var elasticsearch = new ElasticsearchClient();
            elasticsearch.relationName = type.GetRelationName();
            elasticsearch.indiceName = type.GetIndicesName(state ?? DateTime.Now);
            if (elasticsearch.indiceName.IsNullOrEmpty())
                throw new ElasticsearchError("索引名称不可为空");

            elasticsearch.elasticClient = new ElasticClient(_options.ConnectionSettings.DefaultIndex(elasticsearch.indiceName));

            if (!elasticsearch.ExistsIndices(elasticsearch.indiceName))
            {
                if (!type.IsAutoCreate())
                    throw new ElasticsearchError("索引不存在");
                var create = elasticsearch.elasticClient.Indices.Create(
                    elasticsearch.indiceName,
                    i => i.Settings(s =>
                            s.NumberOfShards(_options.NumberOfShards)
                            .NumberOfReplicas(_options.NumberOfReplicas))
                        .Map<T>(m =>
                        {
                            m = m.AutoMap();
                            if (type.IsDynamic())
                                m = m.Dynamic(true);
                            return m;
                        }));
                if (!create.IsValid)
                    throw new ElasticsearchError(create.ServerError.Error.Reason, create.DebugInformation);
            }

            if (elasticsearch.relationName != elasticsearch.indiceName)
                elasticsearch.CreateAlias(elasticsearch.relationName, true);

            return elasticsearch;
        }
    }
}
