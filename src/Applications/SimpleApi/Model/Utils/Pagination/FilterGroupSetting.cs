using Microservice.Library.OpenApi.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Model.Utils.Pagination
{
    /// <summary>
    /// 分组设置
    /// </summary>
    [OpenApiSchemaStrictMode]
    [OpenApiMainTag("FilterGroupSetting")]
    public class FilterGroupSetting
    {
        /// <summary>
        /// 分组标识
        /// <para>默认值 keep</para>
        /// <para>用于标识分组的开始和结束</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@enum, OpenApiSchemaFormat.enum_description, FilterGroupFlag.keep)]
        [JsonConverter(typeof(StringEnumConverter))]
        public FilterGroupFlag Flag { get; set; } = FilterGroupFlag.keep;

        /// <summary>
        /// 组内关系
        /// <para>默认值 and</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@enum, OpenApiSchemaFormat.enum_description, FilterGroupRelation.and)]
        [JsonConverter(typeof(StringEnumConverter))]
        public FilterGroupRelation Relation { get; set; } = FilterGroupRelation.and;
    }
}
