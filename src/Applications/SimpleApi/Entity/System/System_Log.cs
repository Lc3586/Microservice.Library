using FreeSql.DataAnnotations;
using Microservice.Library.Elasticsearch.Annotations;
using Microservice.Library.OpenApi.Annotations;
using Nest;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Entity.System
{
    /// <summary>
    /// 系统日志
    /// </summary>
    [Table]
    [OraclePrimaryKeyName("pk_" + nameof(System_Log))]
    #region 设置索引
    [Index(nameof(System_Log) + "_idx_" + nameof(Level), nameof(Level) + " ASC")]
    [Index(nameof(System_Log) + "_idx_" + nameof(LogType), nameof(LogType) + " ASC")]
    [Index(nameof(System_Log) + "_idx_" + nameof(CreatorId), nameof(CreatorId) + " ASC")]
    [Index(nameof(System_Log) + "_idx_" + nameof(CreateTime), nameof(CreateTime) + " DESC")]
    #endregion
    #region Elasticsearch相关
    [ElasticsearchIndiceExtension(Version = "v1", IndicesSubType = NestIndexSubType.Month)]
    [ElasticsearchType(RelationName = nameof(System_Log), IdProperty = nameof(Id))]
    #endregion
    public class System_Log
    {
        /// <summary>
        /// Id
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Column(IsPrimary = true, StringLength = 36)]
        [Keyword]
        public string Id { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Column(StringLength = 10)]
        [Keyword]
        public string Level { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Column(StringLength = 20)]
        [Keyword]
        public string LogType { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Column(StringLength = -2)]
        [Text]
        public string LogContent { get; set; }

        /// <summary>
        /// 数据备份（转为JSON字符串）
        /// </summary>
        [OpenApiSubTag("Detail")]
        [Column(StringLength = -2)]
        [Text]
        public string Data { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        [Column(StringLength = 36)]
        public string CreatorId { get; set; }

        /// <summary>
        /// 操作者名称
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string)]
        [Description("操作者")]
        [Column(StringLength = 50)]
        public string CreatorName { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(Microservice.Library.OpenApi.JsonExtension.DateTimeConverter), "yyyy-MM-dd HH:mm:ss.ffff")]
        [Description("操作时间")]
        [Date(Format = "dateOptionalTime")]
        public DateTime CreateTime { get; set; }

        #region 关联

        /// <summary>
        /// 用户
        /// </summary>
        [Navigate(nameof(CreatorId))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual System_User User { get; set; }

        #endregion
    }
}
