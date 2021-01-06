using FreeSql.DataAnnotations;
using Library.Elasticsearch.Annotations;
using Library.OpenApi.Annotations;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
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
    [Index(nameof(System_Log) + "_idx_" + nameof(OperatorId), nameof(OperatorId) + " ASC")]
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
        public string Id { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Keyword]
        public string Level { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Keyword]
        public string LogType { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Text]
        [Column(StringLength = -2)]
        public string LogContent { get; set; }

        /// <summary>
        /// 数据备份（转为JSON字符串）
        /// </summary>
        [OpenApiSubTag("Detail")]
        [Text]
        public string Data { get; set; }

        /// <summary>
        /// 操作者Id
        /// </summary>
        [OpenApiSubTag("_List")]
        [Keyword]
        [Column(StringLength = 36)]
        public string OperatorId { get; set; }

        /// <summary>
        /// 操作者名称
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Description("操作者")]
        [Keyword]
        [Column(StringLength = 50)]
        public string OperatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(Library.Json.Converters.DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        [Description("创建时间")]
        [Date(Format = "yyyy-MM-dd HH:mm:ss.ffff")]
        public DateTime CreateTime { get; set; }
    }
}
