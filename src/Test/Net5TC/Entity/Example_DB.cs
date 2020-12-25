using FreeSql.DataAnnotations;
using Library.OpenApi.Annotations;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Net5TC.Entity
{
    /// <summary>
    /// 示例实体类（数据库）
    /// </summary>
    /// <remarks>基础</remarks>
    [Table]//FreeSql使用CodeFirst模式时必须添加此特性，如果要禁用实体同步设置DisableSyncStructure = true
    [OraclePrimaryKeyName("pk_" + nameof(Example_DB))]
    //[System.ComponentModel.DataAnnotations.Schema.Table(nameof(Example_DB))]//特别指定数据库表名
    #region 设置索引
    [Index(nameof(Example_DB) + "_idx_" + nameof(Name), nameof(Name) + " ASC")]
    [Index(nameof(Example_DB) + "_idx_" + nameof(CreatorId), nameof(CreatorId) + " ASC")]
    [Index(nameof(Example_DB) + "_idx_" + nameof(CreatorName), nameof(CreatorName) + " ASC")]
    [Index(nameof(Example_DB) + "_idx_" + nameof(CreateTime), nameof(CreateTime) + " DESC")]
    #endregion
    public class Example_DB
    {
        /// <summary>
        /// Id
        /// </summary>
        [Column(IsPrimary = true, StringLength = 36)]//设置主键
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [OpenApiSubTag("List", "Create", "Edit", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string)]
        [Required(ErrorMessage = "名称不可为空")]//非空验证（设置此特性时，如果Column特性中未设置IsNullable=true，那么数据库结构会同步为非空）
        [Description("名称")]
        [Column(StringLength = 50)]//字符串类型同步至数据库结构默认可为空
        public string Name { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [OpenApiSubTag("Create", "Edit", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string)]
        [Required(ErrorMessage = "内容不可为空", AllowEmptyStrings = true)]//非空验证,允许空字符串
        [Description("内容")]
        [Column(StringLength = -2)]
        public string Content { get; set; }

        /// <summary>
        /// 值1
        /// </summary>
        [OpenApiSubTag("Create", "Edit", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string)]
        [Description("值1")]
        [Column(IsNullable = true)]
        public long? Value1 { get; set; }

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
        [JsonConverter(typeof(Library.Json.Converters.DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近修改时间
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, "", OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(Library.Json.Converters.DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        [Description("最近修改时间")]
        public DateTime ModifyTime { get; set; }
    }
}
