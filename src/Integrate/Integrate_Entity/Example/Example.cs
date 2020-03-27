using Library.Elasticsearch.Annotations;
using Library.Json.Converters;
using Library.OpenApi.Annotations;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Integrate_Entity.Example
{
    /// <summary>
    /// 示例实体类
    /// </summary>
    [FreeSql.DataAnnotations.Table(DisableSyncStructure = true)]//FreeSql使用CodeFirst模式时必须添加此特性
    //[System.ComponentModel.DataAnnotations.Schema.Table("Example")]//数据库表名
    [ElasticsearchIndiceExtention(Version = "v1", IndicesSubType = NestIndexSubType.None)]//搜索服务索引拓展功能
    [ElasticsearchType(RelationName = "Example", IdProperty = nameof(Id))]//搜索服务索引类型信息
    public class Example
    {
        /// <summary>
        /// Id
        /// </summary>
        #region Elasticsearch相关
        [Number(NumberType.Long)]//搜索服务文档类型
        #endregion
        #region Json相关
        [JsonIgnore]
        #endregion
        #region FreeSql CodeFirst模式
        //[FreeSql.DataAnnotations.Column(IsPrimary = true)]
        #endregion
        [OpenApiSubTag("_List")]
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Keyword]
        [OpenApiSubTag("List", "Create", "Edit", "Detail")]
        [OpenApiSchema(OpenApiSchemaType._string)]
        [Required(ErrorMessage = "名称不可为空")]//非空验证
        public string Name { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Text]
        [OpenApiSubTag("Create", "Edit", "Detail")]
        [OpenApiSchema(OpenApiSchemaType._string)]
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
        [OpenApiSchema(OpenApiSchemaType._string)]
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Date(Format = "yyyy-MM-dd HH:mm:ss")]
        [OpenApiSubTag("List", "Detail")]
        [OpenApiSchema(OpenApiSchemaType._string, OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近编辑时间
        /// </summary>
        [Date(Format = "yyyy-MM-dd HH:mm:ss")]
        [OpenApiSubTag("List", "Detail", "_Edit")]
        [OpenApiSchema(OpenApiSchemaType._string, "", OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime ModifyTime { get; set; }
    }
}
