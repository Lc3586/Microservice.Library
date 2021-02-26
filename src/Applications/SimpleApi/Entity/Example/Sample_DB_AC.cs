using FreeSql.DataAnnotations;
using Microservice.Library.OpenApi.Annotations;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Entity.Example
{
    /// <summary>
    /// 示例实体类-模块信息（数据库）
    /// </summary>
    /// <remarks>
    /// <para>附带导航属性，联合主键，一对一</para>
    /// <para>联合主键</para>
    /// </remarks>
    [Table]//FreeSql使用CodeFirst模式时必须添加此特性，如果要禁用实体同步设置DisableSyncStructure = true
    //[System.ComponentModel.DataAnnotations.Schema.Table(nameof(Example_DB_AC))]//特别指定数据库表名
    #region 设置索引
    [Index(nameof(Sample_DB_AC) + "_idx_" + nameof(CreatorId), nameof(CreatorId) + " ASC")]
    [Index(nameof(Sample_DB_AC) + "_idx_" + nameof(CreatorName), nameof(CreatorName) + " ASC")]
    [Index(nameof(Sample_DB_AC) + "_idx_" + nameof(CreateTime), nameof(CreateTime) + " DESC")]
    #endregion
    public class Sample_DB_AC
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <remarks>联合主键</remarks>
        [Column(IsPrimary = true, StringLength = 36)]//设置主键
        public string AId { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        /// <remarks>联合主键</remarks>
        [Column(IsPrimary = true, StringLength = 36)]//设置主键
        public string CId { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [Column(StringLength = 36)]
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string)]
        [Description("创建者")]
        [Column(StringLength = 50)]
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(Microservice.Library.Json.Converters.DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }

        #region 关联

        /// <summary>
        /// 相关A
        /// </summary>
        /// <remarks>一对一</remarks>
        [Navigate(nameof(AId))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual Sample_DB_A Example_DB_A { get; set; }

        /// <summary>
        /// 相关C
        /// </summary>
        /// <remarks>一对一</remarks>
        [Navigate(nameof(CId))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual Sample_DB_C Example_DB_C { get; set; }

        #endregion
    }
}
