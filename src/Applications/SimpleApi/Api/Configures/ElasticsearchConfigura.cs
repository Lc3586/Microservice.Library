using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Model.System;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Configures
{
    /// <summary>
    /// ES搜索服务配置类
    /// </summary>
    public class ElasticsearchConfigura
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void RegisterServices(IServiceCollection services, SystemConfig config)
        {
            services.AddElasticsearch(s =>
            {
                if (config.ElasticsearchNodes != null)
                    if (config.ElasticsearchNodes.Count() == 1)
                        s.ElasticsearchGeneratorOptions.ConnectionSettings = new ConnectionSettings(config.ElasticsearchNodes[0]);
                    else
                        s.ElasticsearchGeneratorOptions.ConnectionSettings = new ConnectionSettings(new StaticConnectionPool(config.ElasticsearchNodes));
            });
        }
    }
}
