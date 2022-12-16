using Microservice.Library.Elasticsearch;
using Microservice.Library.Elasticsearch.Gen;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ElasticsearchServiceCollectionExtensions
    {
        /// <summary>
        /// 注册ES
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddElasticsearch(
            this IServiceCollection services,
            Action<ElasticsearchGenOptions> setupAction = null)
        {
            //注册自定义配置程序，将高级配置（ElasticsearchGenOptions）应用于低级配置（ElasticsearchGeneratorOptions）。
            services.AddTransient<IConfigureOptions<ElasticsearchGeneratorOptions>, ConfigureElasticsearchGeneratorOptions>();

            //注册生成器和依赖
            services.AddTransient(s => s.GetRequiredService<IOptions<ElasticsearchGeneratorOptions>>().Value);
            services.AddTransient<IElasticsearchProvider, ElasticsearchGenerator>();

            if (setupAction != null) services.ConfigureElasticsearch(setupAction);

            return services;
        }

        public static void ConfigureElasticsearch(
            this IServiceCollection services,
            Action<ElasticsearchGenOptions> setupAction)
        {
            services.Configure(setupAction);
        }
    }
}
