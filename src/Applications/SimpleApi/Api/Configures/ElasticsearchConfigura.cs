using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Model.System.Config;
using Nest;
using System.Linq;

namespace Api.Configures
{
    /// <summary>
    /// ES搜索服务配置类
    /// </summary>
    public static class ElasticsearchConfigura
    {
        /// <summary>
        /// 注册ES搜索服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static IServiceCollection RegisterElasticsearch(this IServiceCollection services, SystemConfig config)
        {
            services.AddElasticsearch(s =>
            {
                if (config.ElasticsearchNodes != null)
                    if (config.ElasticsearchNodes.Count() == 1)
                        s.ElasticsearchGeneratorOptions.ConnectionSettings = new ConnectionSettings(config.ElasticsearchNodes[0]);
                    else
                        s.ElasticsearchGeneratorOptions.ConnectionSettings = new ConnectionSettings(new StaticConnectionPool(config.ElasticsearchNodes));
            });

            return services;
        }
    }
}
