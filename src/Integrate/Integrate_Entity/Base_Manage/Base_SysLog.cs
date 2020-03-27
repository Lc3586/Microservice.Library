using Library.Elasticsearch;
using Nest;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Integrate_Entity.Base_Manage
{
    /// <summary>
    /// 系统日志表
    /// </summary>
    [Table("Base_Log")]
    [ElasticsearchIndices(IndicesSubType = NestIndexSubType.Month)]
    [ElasticsearchType(RelationName = "Base_Log", IdProperty = nameof(Id))]
    public class Base_Log
    {

        /// <summary>
        /// 自然主键
        /// </summary>
        [Key, Column(Order = 1)]
        [Keyword]
        public String Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Date]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        [Keyword]
        public String CreatorId { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        [Keyword]
        public String CreatorRealName { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
        [Boolean]
        public Boolean Deleted { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        [Keyword]
        public String Level { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        [Keyword]
        public String LogType { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        [Text]
        public String LogContent { get; set; }

        /// <summary>
        /// 数据备份（转为JSON字符串）
        /// </summary>
        [Text]
        public String Data { get; set; }

    }
}