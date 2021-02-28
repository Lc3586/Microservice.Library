namespace Microservice.Library.OpenApi.JsonExtension
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
