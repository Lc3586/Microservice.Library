using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    /// <summary>
    /// Swagger实现文件上传功能
    /// </summary>
    public class SwaggerFileUploadFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //if (!context.ApiDescription.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase) &&
            //  !context.ApiDescription.HttpMethod.Equals("PUT", StringComparison.OrdinalIgnoreCase))
            //    return;

            //var fileParameters = context.ApiDescription.ActionDescriptor.Parameters.Where(n => n.ParameterType == typeof(IFormFile)).ToList();

            //operation.Consumes.Add("multipart/form-data");

            //foreach (var fileParameter in fileParameters)
            //{
            //    var parameter = operation.Parameters.Single(n => n.Name == fileParameter.Name);
            //    operation.Parameters.Remove(parameter);
            //    operation.Parameters.Add(new NonBodyParameter
            //    {
            //        Name = parameter.Name,
            //        In = "formData",
            //        Description = parameter.Description,
            //        Required = parameter.Required,
            //        Type = "file"
            //    });
            //}
        }
    }
}
