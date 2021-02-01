using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Model.System.Config;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Api.Configures
{
    /// <summary>
    /// 配置Swagger选项
    /// </summary>
    /// <remarks>当Api有多个版本时使用此类,将系统配置应用于Swagger配置</remarks>
    public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IApiVersionDescriptionProvider ApiVersionProvider;

        readonly SwaggerApiMultiVersionDescriptionOptions Options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="options"></param>
        public ConfigureSwaggerGenOptions(
           IServiceProvider serviceProvider,
           IOptions<SwaggerApiMultiVersionDescriptionOptions> options)
        {
            ServiceProvider = serviceProvider;
            ApiVersionProvider = ServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
            Options = options.Value;
        }

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="options"></param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in ApiVersionProvider.ApiVersionDescriptions)
            {
                var apiVersion = Options.ApiMultiVersionDescription.Find(o => o.GroupName == description.GroupName);
                options.SwaggerDoc(
                    description.GroupName,
                    new OpenApiInfo()
                    {
                        Title = apiVersion.Title,
                        Version = apiVersion.Version ?? description.ApiVersion.ToString(),
                        Description = apiVersion.Description
                    });
            }
        }
    }
}
