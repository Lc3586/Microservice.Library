using System.Collections.Generic;

namespace Microservice.Library.OpenApi.JsonExtension
{
    /// <summary>
    /// 自定义解析器
    /// </summary>
    /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
    public class OpenApiContractResolver<TOpenApiSchema> : OpenApiContractResolver
    {
        /// <summary>
        /// 
        /// </summary>
        public OpenApiContractResolver()
            : base(typeof(TOpenApiSchema))
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyDic">输出的属性</param>
        public OpenApiContractResolver(Dictionary<string, List<string>> propertyDic)
            : base(propertyDic)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionProperties">特别输出的属性</param>
        /// <param name="ignoreProperties">特别忽略的属性</param>
        public OpenApiContractResolver(Dictionary<string, List<string>> exceptionProperties, Dictionary<string, List<string>> ignoreProperties)
            : base(typeof(TOpenApiSchema), exceptionProperties, ignoreProperties)
        {

        }
    }
}
