using Microservice.Library.Container;
using Microsoft.OpenApi.Models;
using Model.Utils.Config;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Api
{
    /// <summary>
    /// 接口文档过滤器
    /// </summary>
    /// <remarks>LCTR 2021-02-21</remarks>
    public class DocumentFilter : IDocumentFilter
    {
        SystemConfig Config => AutofacHelper.GetScopeService<SystemConfig>();

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var removePaths = new List<string>();

            if (!Config.EnableCAS)
            {
                removePaths.AddRange(context.ApiDescriptions.Where(o => o.RelativePath.IndexOf("cas/") == 0)
                    .Select(o => o.RelativePath));
            }

            if (!Config.EnableSampleAuthentication)
            {
                removePaths.AddRange(context.ApiDescriptions.Where(o => o.RelativePath.IndexOf("sa/") == 0)
                    .Select(o => o.RelativePath));
            }

            if (!Config.EnableWeChatService)
            {
                removePaths.AddRange(context.ApiDescriptions.Where(o => o.RelativePath.IndexOf("wechat-user/") == 0
                                                                        || o.RelativePath.IndexOf("wechat-oath/") == 0)
                    .Select(o => o.RelativePath));
            }

            if (removePaths.Any())
                removePaths.ForEach(o => swaggerDoc.Paths.Remove($"/{o}"));
        }
    }
}
