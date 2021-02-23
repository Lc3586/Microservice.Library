using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Model.Utils.Config;
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
                if (config.ESNodes != null)
                    if (config.ESNodes.Count() == 1)
                        s.ElasticsearchGeneratorOptions.ConnectionSettings = new ConnectionSettings(config.ESNodes[0]);
                    else
                        s.ElasticsearchGeneratorOptions.ConnectionSettings = new ConnectionSettings(new StaticConnectionPool(config.ESNodes));

                switch (config.ESSecurityType)
                {
                    case ESSecurityType.Basic:
                        s.ElasticsearchGeneratorOptions.ConnectionSettings.BasicAuthentication(config.ESUserName, config.ESPassword);
                        break;
                    case ESSecurityType.ApiKey:
                        s.ElasticsearchGeneratorOptions.ConnectionSettings.ApiKeyAuthentication(config.ESKeyId, config.ESApiKey);
                        break;
                    case ESSecurityType.None:
                    default:
                        break;
                }
            });

            return services;
        }
    }
}
