using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerGen;
using Integrate_Business.Config;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Http;

namespace Integrate_Api
{
    public class SwaggerConfigura
    {
        public static void RegisterServices(IServiceCollection services)
        {
            #region 多版本文档

            //services.AddApiVersioning();
            //services.AddVersionedApiExplorer();

            #endregion

            services.AddSwaggerGen(s =>
            {
                #region 配置文档

                //单文档配置方式
                s.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Title = SystemConfig.systemConfig.ProjectName,
                    Version = "v1.0",
                    Description = "接口文档"
                });

                //多版本文档配置方式
                //services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

                #endregion

                #region 自定义架构Id选择器

                Func<Type, string> SchemaIdSelector = null;
                SchemaIdSelector = (Type modelType) =>
                {
                    if (!modelType.IsConstructedGenericType) return modelType.FullName.Replace("[]", "Array");

                    var prefix = modelType.GetGenericArguments()
                        .Select(genericArg => SchemaIdSelector(genericArg))
                        .Aggregate((previous, current) => previous + current);

                    return prefix + modelType.FullName.Split('`').First();
                };
                s.CustomSchemaIds(SchemaIdSelector);

                #endregion

                s.SchemaFilter<OpenApiSchemaFilter>();

                #region 为JSON文件和UI设置xml文档路径

                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                foreach (var item in new[] { "Integrate_Entity.xml", "Integrate_Model.xml", "Integrate_Business.xml", "Integrate_Api.xml", "Library.Models.xml" })
                {
                    var xmlPath = Path.Combine(basePath, item);

                    if (File.Exists(xmlPath))
                        s.IncludeXmlComments(xmlPath);
                }

                #endregion

                //启用注解
                s.EnableAnnotations();
            });
        }

        public static void RegisterApplication(IApplicationBuilder app/*, IApiVersionDescriptionProvider provider*/)
        {
            #region 用户语言（展示用，普通项目无需添加此内容）

            var supportedCultures = new[]
{
                new CultureInfo("en-US"),
                new CultureInfo("fr"),
                new CultureInfo("sv-SE"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
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
                            Url = SystemConfig.systemConfig.PublishRootUrl,
                            Description = "服务器地址"
                        }
                    };
                });
            });
            app.UseSwaggerUI(s =>
            {
                //多版本文档
                //foreach (var description in provider.ApiVersionDescriptions)
                //{
                //    s.SwaggerEndpoint($"{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                //}
                s.SwaggerEndpoint("/swagger/v1.0/swagger.json", "v1.0 文档");

                #region 页面自定义选项

                s.DocumentTitle = $"{SystemConfig.systemConfig.ProjectName}接口文档";//页面标题
                s.DisplayOperationId();//显示操作Id
                s.DisplayRequestDuration();//显示请求持续时间
                s.EnableFilter();//启用顶部筛选框
                //s.InjectStylesheet("/swagger/custom-stylesheet.css");//自定义样式表，需要启用静态文件
                //s.InjectJavascript("/swagger/custom-javascript.js");//自定义脚本，需要启用静态文件
                s.InjectJavascript("/swagger/jquery-1.8.3.min.js");
                s.InjectStylesheet("/swagger/waiting.css");
                s.InjectJavascript("/swagger/waiting.min.js");
                s.InjectJavascript("/swagger/casLogin.js");//cas登录脚本脚本

                #endregion

                s.RoutePrefix = string.Empty;
            });
        }
    }
}
