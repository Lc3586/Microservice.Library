using Library.OpenApi.Annotations;

namespace Model.Utils.Result
{
    /// <summary>
    /// Ajax请求结果
    /// </summary>
    public class AjaxResult<T> : AjaxResult
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public T Data { get; set; }
    }
}
