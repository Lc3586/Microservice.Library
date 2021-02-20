using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Model.System.Config;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Api.Configures
{
    /// <summary>
    /// Swagger单文档配置类
    /// </summary>
    public static class SwaggerConfigura
    {
        /// <summary>
        /// 注册Swagger服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static IServiceCollection RegisterSwagger(this IServiceCollection services, SystemConfig config)
        {
            services.AddMvc()
                //禁用框架结构属性小驼峰命名规则
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddSwaggerGen(s =>
            {
                #region 配置文档

                s.SwaggerDoc(config.SwaggerApiVersion.Version, new OpenApiInfo
                {
                    Title = config.SwaggerApiVersion.Title,
                    Version = config.SwaggerApiVersion.Version,
                    Description = config.SwaggerApiVersion.Description
                });

                #endregion

                #region 自定义架构Id选择器

                static string SchemaIdSelector(Type modelType)
                {
                    if (!modelType.IsConstructedGenericType) return modelType.FullName.Replace("[]", "Array");

                    var prefix = modelType.GetGenericArguments()
                        .Select(genericArg => SchemaIdSelector(genericArg))
                        .Aggregate((previous, current) => previous + current);

                    return prefix + modelType.FullName.Split('`').First();
                }

                s.CustomSchemaIds(SchemaIdSelector);

                #endregion

                s.SchemaFilter<OpenApiSchemaFilter>();

                #region 为JSON文件和UI设置xml文档路径

                //获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                foreach (var item in config.SwaggerXmlComments)
                {
                    var xmlPath = Path.Combine(basePath, item);

                    if (File.Exists(xmlPath))
                        s.IncludeXmlComments(xmlPath);
                }

                #endregion

                //启用注解
                s.EnableAnnotations();
            });

            return services;
        }

        /// <summary>
        /// 配置Swagger
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        public static IApplicationBuilder ConfiguraSwagger(this IApplicationBuilder app, SystemConfig config)
        {
            #region 用户语言（展示用，普通项目无需添加此内容）

            var supportedCultures = new[]
{
                new CultureInfo("zh-CN"),
                new CultureInfo("en-US"),
                new CultureInfo("fr"),
                new CultureInfo("sv-SE"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("zh-CN"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            #endregion

            app.UseSwagger(s =>
            {
                s.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Servers = new List<OpenApiServer> {
                        new OpenApiServer {
                            Url = $"{httpReq.Scheme}://{httpReq.Host.Value}",
                            Description = "当前地址"
                        },
                        new OpenApiServer {
                            Url = config.PublishRootUrl,
                            Description = "服务器地址"
                        }
                    };
                });
            });
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint($"/swagger/{config.SwaggerApiVersion.Version}/swagger.json", config.SwaggerApiVersion.Name);

                #region 页面自定义选项

                s.DocumentTitle = config.SwaggerApiVersion.Title;//页面标题
                s.DisplayOperationId();//显示操作Id
                s.DisplayRequestDuration();//显示请求持续时间
                s.EnableFilter();//启用顶部筛选框
                //s.InjectStylesheet("/swagger/custom-stylesheet.css");//自定义样式表，需要启用静态文件
                //s.InjectJavascript("/swagger/custom-javascript.js");//自定义脚本，需要启用静态文件
                s.InjectJavascript("/swagger/jquery-1.8.3.min.js");
                s.InjectStylesheet("/swagger/waiting.css");
                s.InjectJavascript("/swagger/waiting.min.js");
                s.InjectStylesheet("/swagger/custom-stylesheet.css");
                s.InjectJavascript("/swagger/custom-javascript.js");
                if (config.EnableCAS)
                    s.InjectJavascript("/swagger/casLogin.js");//cas登录脚本脚本
                if (config.EnableSampleAuthentication)
                    s.InjectJavascript("/swagger/saLogin.js");//sa登录脚本脚本

                #endregion

                s.RoutePrefix = string.Empty;
            });

            return app;
        }
    }
}
