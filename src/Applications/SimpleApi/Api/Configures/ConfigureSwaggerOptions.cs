using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Model.System;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Configures
{
    /// <summary>
    /// 配置Swagger选项
    /// </summary>
    /// <remarks>当Api有多个版本时使用此类,将系统配置应用于Swagger配置</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider Provider;

        readonly SystemConfig Config;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="options"></param>
        public ConfigureSwaggerOptions(
           IApiVersionDescriptionProvider serviceProvider,
           IOptions<SystemConfig> options)
        {
            Provider = serviceProvider;
            Config = options.Value;
        }

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="options"></param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in Provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    new OpenApiInfo()
                    {
                        Title = $"{Config.ProjectName} API - {description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                        Description = "接口文档"
                    });
            }
        }
    }
}
