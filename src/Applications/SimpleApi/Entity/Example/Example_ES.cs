using Library.Elasticsearch.Annotations;
using Library.Json.Converters;
using Library.OpenApi.Annotations;
using Nest;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entity.Example
{
    /// <summary>
    /// 示例实体类（ES服务）
    /// </summary>
    [ElasticsearchIndiceExtension(Version = "v1", IndicesSubType = NestIndexSubType.None)]//搜索服务索引拓展功能
    [ElasticsearchType(RelationName = nameof(Example_ES), IdProperty = nameof(Id))]//搜索服务索引类型信息
    public class Example_ES
    {
        /// <summary>
        /// Id
        /// </summary>
        [Keyword]//搜索服务文档类型
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Keyword]
        [OpenApiSubTag("List", "Create", "Edit", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string)]
        [Required(ErrorMessage = "名称不可为空")]//非空验证
        public string Name { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Text]
        [OpenApiSubTag("Create", "Edit", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string)]
        [Required(ErrorMessage = "内容不可为空", AllowEmptyStrings = true)]//非空验证,允许空字符串
        public string Content { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [Keyword]
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        [Keyword]
        [OpenApiSubTag("List", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string)]
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Date(Format = "yyyy-MM-dd HH:mm:ss")]
        [OpenApiSubTag("List", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近编辑时间
        /// </summary>
        [Date(Format = "yyyy-MM-dd HH:mm:ss")]
        [OpenApiSubTag("List", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, "", OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime ModifyTime { get; set; }
    }
}
