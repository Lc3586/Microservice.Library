using Library.OpenApi.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Model.System.Pagination
{
    /// <summary>
    /// 分页高级排序
    /// </summary>
    [OpenApiSchemaStrictMode]
    [OpenApiMainTag("PaginationAdvancedSort")]
    public class PaginationAdvancedSort
    {
        /// <summary>
        /// 字段
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@string)]
        public string Field { get; set; }

        /// <summary>
        /// 类型
        /// <para>默认值 desc</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@enum, OpenApiSchemaFormat.enum_description, SortType.desc)]
        [JsonConverter(typeof(StringEnumConverter))]
        public SortType Type { get; set; } = SortType.desc;
    }
}
