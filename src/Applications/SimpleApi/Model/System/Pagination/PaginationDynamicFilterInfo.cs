using Library.OpenApi.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Model.System.Pagination
{
    /// <summary>
    /// 分页筛选条件
    /// </summary>
    [OpenApiSchemaStrictMode]
    [OpenApiMainTag("PaginationDynamicFilterInfo")]
    public class PaginationDynamicFilterInfo
    {
        /// <summary>
        /// 要比较的字段
        /// <para>注意区分大小写</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@string)]
        public string Field { get; set; }

        /// <summary>
        /// 用于比较的值
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@string)]
        public object Value { get; set; }

        /// <summary>
        /// 比较类型
        /// <para>默认值 eq</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@enum, OpenApiSchemaFormat.enum_description, FilterCompare.eq)]
        [JsonConverter(typeof(StringEnumConverter))]
        public FilterCompare Compare { get; set; } = FilterCompare.eq;

        /// <summary>
        /// 组内关系
        /// <para>默认值 and</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@enum, OpenApiSchemaFormat.enum_description, FilterGroupRelation.and)]
        [JsonConverter(typeof(StringEnumConverter))]
        public FilterGroupRelation Relation { get; set; } = FilterGroupRelation.and;

        /// <summary>
        /// 分组设置
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model, OpenApiSchemaFormat.model_once)]
        public List<PaginationDynamicFilterInfo> DynamicFilterInfo { get; set; }
    }
}
