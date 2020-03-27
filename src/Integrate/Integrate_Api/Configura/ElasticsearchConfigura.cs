using Elasticsearch.Net;
using Integrate_Business.Config;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integrate_Api
{
    public class ElasticsearchConfigura
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddElasticsearch(s =>
            {
                if (SystemConfig.systemConfig.ElasticsearchNodes != null)
                    if (SystemConfig.systemConfig.ElasticsearchNodes.Count() == 1)
                        s.ElasticsearchGeneratorOptions.ConnectionSettings = new ConnectionSettings(SystemConfig.systemConfig.ElasticsearchNodes[0]);
                    else
                        s.ElasticsearchGeneratorOptions.ConnectionSettings = new ConnectionSettings(new StaticConnectionPool(SystemConfig.systemConfig.ElasticsearchNodes));
            });
        }
    }
}
