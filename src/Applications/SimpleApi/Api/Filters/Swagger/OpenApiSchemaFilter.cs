using Microservice.Library.OpenApi.Annotations;
using Microservice.Library.OpenApi.Extention;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Api
{
    /// <summary>
    /// 接口架构过滤器
    /// </summary>
    /// <remarks>LCTR 2020-03-10</remarks>
    public class OpenApiSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema == null || context.Type == null || context.Type.GetCustomAttribute<OpenApiIgnoreAttribute>() != null)
                return;
            schema.Example = context.Type.GetOrNullFor();
        }
    }
}
