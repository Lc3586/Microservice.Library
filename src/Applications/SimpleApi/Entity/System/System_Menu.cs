using FreeSql.DataAnnotations;
using Library.OpenApi.Annotations;
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
    /// 菜单
    /// </summary>
    [Table]
    [OraclePrimaryKeyName("pk_" + nameof(System_Menu))]
    #region 设置索引
    [Index(nameof(System_Menu) + "_idx_" + nameof(ParentId), nameof(ParentId) + " ASC")]
    [Index(nameof(System_Menu) + "_idx_" + nameof(Name), nameof(Name) + " ASC")]
    [Index(nameof(System_Menu) + "_idx_" + nameof(Type), nameof(Type) + " ASC")]
    [Index(nameof(System_Menu) + "_idx_" + nameof(Enable), nameof(Enable) + " DESC")]
    [Index(nameof(System_Menu) + "_idx_" + nameof(CreatorId), nameof(CreatorId) + " ASC")]
    [Index(nameof(System_Menu) + "_idx_" + nameof(CreatorName), nameof(CreatorName) + " ASC")]
    [Index(nameof(System_Menu) + "_idx_" + nameof(CreateTime), nameof(CreateTime) + " DESC")]
    #endregion
    public class System_Menu
    {
        /// <summary>
        /// Id
        /// </summary>
        [OpenApiSubTag("List", "TreeList", "Edit", "Detail", "Authorities", "Sort")]
        [Column(IsPrimary = true, StringLength = 36)]
        public string Id { get; set; }

        /// <summary>
        /// 根菜单Id
        /// </summary>
        [OpenApiSubTag("List", "TreeList", "_Edit", "Sort")]
        [Column(StringLength = 36)]
        public string RootId { get; set; }

        /// <summary>
        /// 父级菜单Id
        /// </summary>
        [OpenApiSubTag("List", "TreeList", "Edit", "Sort")]
        [Column(StringLength = 36)]
        public string ParentId { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        [OpenApiSubTag("List", "TreeList", "_Edit", "Sort")]
        public int Level { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [OpenApiSubTag("List", "TreeList", "Create", "Edit", "Detail", "Authorities")]
        [Description("名称")]
        [Column(StringLength = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [OpenApiSubTag("List", "TreeList", "Create", "Edit", "Detail", "Authorities")]
        [Description("类型")]
        [Column(StringLength = 20)]
        public string Type { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [OpenApiSubTag("List", "TreeList", "Create", "Edit", "Detail", "Authorities")]
        [Description("编码")]
        [Column(StringLength = 20)]
        public string Code { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        [OpenApiSubTag("List", "TreeList", "Create", "Edit", "Detail", "Authorities")]
        [Description("链接地址")]
        [Column(StringLength = 2048)]
        public string Uri { get; set; }

        /// <summary>
        /// 请求方法
        /// </summary>
        [OpenApiSubTag("List", "TreeList", "Create", "Edit", "Detail", "Authorities")]
        [Description("请求方法")]
        [Column(StringLength = 2048)]
        public string Method { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [OpenApiSubTag("List", "TreeList", "Create", "Edit", "Detail")]
        [Description("图标")]
        [Column(StringLength = 36)]
        public string Icon { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        [OpenApiSubTag("List", "TreeList", "Create", "Edit", "Detail")]
        [Description("启用")]
        public bool Enable { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        [OpenApiSubTag("List", "TreeList", "Sort")]
        [Description("排序值")]
        public int Sort { get; set; }

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
        [OpenApiSubTag("List", "TreeList", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string)]
        [Description("创建者")]
        [Column(StringLength = 50)]
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [OpenApiSubTag("List", "TreeList", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(Library.Json.Converters.DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近修改时间
        /// </summary>
        [OpenApiSubTag("List", "TreeList", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, "", OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(Library.Json.Converters.DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        [Description("最近修改时间")]
        public DateTime ModifyTime { get; set; }

        #region 关联

        /// <summary>
        /// 根菜单
        /// </summary>
        [Navigate(nameof(RootId))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual System_Menu Root { get; set; }

        /// <summary>
        /// 父级菜单
        /// </summary>
        [Navigate(nameof(ParentId))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual System_Menu Parent { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        [Navigate(nameof(ParentId))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual ICollection<System_Menu> Childs { get; set; }

        /// <summary>
        /// 被授权此菜单的用户
        /// </summary>
        [Navigate(ManyToMany = typeof(System_UserMenu))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual ICollection<System_User> Users { get; set; }

        /// <summary>
        /// 被授权此菜单的角色
        /// </summary>
        [Navigate(ManyToMany = typeof(System_UserRole))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual ICollection<System_Role> Role { get; set; }

        #endregion
    }
}
