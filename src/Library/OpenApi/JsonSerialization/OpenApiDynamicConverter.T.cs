using Microservice.Library.OpenApi.Annotations;
using Microservice.Library.OpenApi.Extention;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microservice.Library.OpenApi.JsonSerialization
{
    /// <summary>
    /// 自定义动态类型转换器
    /// </summary>
    /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
    internal class OpenApiDynamicConverter<TOpenApiSchema> : OpenApiDynamicConverter
    {
        /// <summary>
        /// 
        /// </summary>
        public OpenApiDynamicConverter()
            : base(typeof(TOpenApiSchema))
        {

        }
    }
}
