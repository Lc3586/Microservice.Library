using FreeSql.DataAnnotations;
using Microservice.Library.OpenApi.Annotations;
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
    /// 资源
    /// </summary>
    [Table]
    [OraclePrimaryKeyName("pk_" + nameof(System_Resources))]
    #region 设置索引
    [Index(nameof(System_Resources) + "_idx_" + nameof(Name), nameof(Name) + " ASC")]
    [Index(nameof(System_Resources) + "_idx_" + nameof(Type), nameof(Type) + " ASC")]
    [Index(nameof(System_Resources) + "_idx_" + nameof(Code), nameof(Code) + " ASC")]
    [Index(nameof(System_Resources) + "_idx_" + nameof(Uri), nameof(Uri) + " ASC")]
    [Index(nameof(System_Resources) + "_idx_" + nameof(Enable), nameof(Enable) + " DESC")]
    [Index(nameof(System_Resources) + "_idx_" + nameof(CreatorId), nameof(CreatorId) + " ASC")]
    [Index(nameof(System_Resources) + "_idx_" + nameof(CreateTime), nameof(CreateTime) + " DESC")]
    [Index(nameof(System_Resources) + "_idx_" + nameof(ModifyTime), nameof(ModifyTime) + " DESC")]
    #endregion
    public class System_Resources
    {
        /// <summary>
        /// Id
        /// </summary>
        [OpenApiSubTag("List", "Edit", "Detail", "Authorities")]
        [Column(IsPrimary = true, StringLength = 36)]
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [OpenApiSubTag("List", "Create", "Edit", "Detail", "Authorities")]
        [Description("名称")]
        [Column(StringLength = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [OpenApiSubTag("List", "Create", "Edit", "Detail", "Authorities")]
        [Description("类型")]
        [Column(StringLength = 20)]
        public string Type { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [OpenApiSubTag("List", "Create", "Edit", "Detail", "Authorities")]
        [Description("编码")]
        [Column(StringLength = 20)]
        public string Code { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        [OpenApiSubTag("List", "Create", "Edit", "Detail", "Authorities")]
        [Description("链接地址")]
        [Column(StringLength = 2048)]
        public string Uri { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [OpenApiSubTag("List", "Create", "Edit", "Detail")]
        [Description("图标")]
        [Column(StringLength = 36)]
        public string Icon { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        [OpenApiSubTag("List", "Create", "Edit", "Detail")]
        [Description("启用")]
        public bool Enable { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [OpenApiSubTag("Create", "Edit", "Detail")]
        [Description("备注")]
        [Column(StringLength = -2)]
        public string Remark { get; set; }

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
        [JsonConverter(typeof(Microservice.Library.OpenApi.JsonExtension.DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近编辑者
        /// </summary>
        [OpenApiSubTag("_Edit")]
        [Column(StringLength = 36)]
        public string ModifiedById { get; set; }

        /// <summary>
        /// 最近编辑者名称
        /// </summary>
        [OpenApiSubTag("List", "Detail", "_Edit")]
        [Description("最近编辑者")]
        [Column(StringLength = 50)]
        public string ModifiedByName { get; set; }

        /// <summary>
        /// 最近编辑时间
        /// </summary>
        [OpenApiSubTag("List", "Detail", "_Edit")]
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(Microservice.Library.OpenApi.JsonExtension.DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        [Description("最近编辑时间")]
        [Column(IsNullable = true)]
        public DateTime? ModifyTime { get; set; }

        #region 关联

        /// <summary>
        /// 被授权此资源的角色
        /// </summary>
        [Navigate(ManyToMany = typeof(System_RoleResources))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual ICollection<System_Role> Roles { get; set; }

        /// <summary>
        /// 被授权此资源的用户
        /// </summary>
        [Navigate(ManyToMany = typeof(System_UserResources))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual ICollection<System_User> Users { get; set; }

        #endregion
    }
}
