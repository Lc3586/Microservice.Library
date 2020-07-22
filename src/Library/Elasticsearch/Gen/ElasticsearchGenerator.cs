using Library.Elasticsearch.Annotations;
using Library.Elasticsearch.Application;
using Library.Extension;
using Nest;
using System;

namespace Library.Elasticsearch.Gen
{
    public class ElasticsearchGenerator : IElasticsearchProvider
    {
        private readonly ElasticsearchGeneratorOptions _options;

        public ElasticsearchGenerator(ElasticsearchGeneratorOptions options)
        {
            _options = options ?? new ElasticsearchGeneratorOptions();
        }

        /// <summary>
        /// 获取ES客户端
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public ElasticsearchClient GetElasticsearch<T>(DateTime? state = null) where T : class
        {
            var type = typeof(T);
            var elasticsearch = new ElasticsearchClient();
            elasticsearch.RelationName = type.GetRelationName();
            elasticsearch.IndiceName = type.GetIndicesName(state ?? DateTime.Now);
            if (elasticsearch.IndiceName.IsNullOrEmpty())
                throw new ElasticsearchException("索引名称不可为空");

            if (ElasticsearchClient.ElasticClient == null)
                ElasticsearchClient.ElasticClient = new ElasticClient(_options.ConnectionSettings);//.DefaultMappingFor<T>(s => s.IndexName(elasticsearch.indiceName)));
            //elasticsearch.elasticClient = new ElasticClient(_options.ConnectionSettings.DefaultIndex(elasticsearch.indiceName));

            if (!elasticsearch.ExistsIndices(elasticsearch.IndiceName))
            {
                if (!type.IsAutoCreate())
                    throw new ElasticsearchException("索引不存在");
                var create = ElasticsearchClient.ElasticClient.Indices.Create(
                    elasticsearch.IndiceName,
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
                    throw new ElasticsearchException(create.ServerError.Error.Reason, create.DebugInformation);
            }

            if (elasticsearch.RelationName != elasticsearch.IndiceName)
                elasticsearch.CreateAlias(elasticsearch.RelationName, true);

            return elasticsearch;
        }
    }
}
