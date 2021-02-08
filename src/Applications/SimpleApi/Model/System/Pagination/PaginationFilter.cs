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
    [OpenApiMainTag("PaginationFilter")]
    public class PaginationFilter
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
        /// Value值是用来比较的字段
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@string)]
        public bool ValueIsField { get; set; }

        /// <summary>
        /// 比较类型
        /// <para>默认值 eq</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@enum, OpenApiSchemaFormat.enum_description, FilterCompare.eq)]
        [JsonConverter(typeof(StringEnumConverter))]
        public FilterCompare Compare { get; set; } = FilterCompare.eq;

        /// <summary>
        /// 分组设置（已弃用）
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public FilterGroupSetting Group { get; set; }

        /// <summary>
        /// 分组设置
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model, OpenApiSchemaFormat.model_once)]
        public List<PaginationFilter> DynamicFilterInfo { get; set; }
    }
}
