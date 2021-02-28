using Entity.Common;
using Entity.System;
using FreeSql.DataAnnotations;
using Microservice.Library.OpenApi.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Entity.Public
{
    /// <summary>
    /// 会员
    /// </summary>
    [Table]
    [OraclePrimaryKeyName("pk_" + nameof(Public_Member))]
    #region 设置索引
    [Index(nameof(Public_Member) + "_idx_" + nameof(Account), nameof(Account) + " ASC")]
    [Index(nameof(Public_Member) + "_idx_" + nameof(Nickname), nameof(Nickname) + " ASC")]
    [Index(nameof(Public_Member) + "_idx_" + nameof(Name), nameof(Name) + " ASC")]
    [Index(nameof(Public_Member) + "_idx_" + nameof(Sex), nameof(Sex) + " ASC")]
    [Index(nameof(Public_Member) + "_idx_" + nameof(Tel), nameof(Tel) + " ASC")]
    [Index(nameof(Public_Member) + "_idx_" + nameof(Company), nameof(Company) + " ASC")]
    [Index(nameof(Public_Member) + "_idx_" + nameof(Enable), nameof(Enable) + " DESC")]
    [Index(nameof(Public_Member) + "_idx_" + nameof(CreatorId), nameof(CreatorId) + " ASC")]
    [Index(nameof(Public_Member) + "_idx_" + nameof(CreateTime), nameof(CreateTime) + " DESC")]
    #endregion
    public class Public_Member
    {
        /// <summary>
        /// Id
        /// </summary>
        [OpenApiSubTag("List", "Edit", "Detail", "Authorities")]
        [Column(IsPrimary = true, StringLength = 36)]
        public string Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [OpenApiSubTag("List", "Create", "Detail", "Authorities")]
        [Description("账号")]
        [Column(StringLength = 50)]
        public string Account { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [OpenApiSubTag("List", "Create", "Edit", "Detail")]
        [Description("昵称")]
        [Column(StringLength = 50)]
        public string Nickname { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [OpenApiSubTag("List", "Create", "Edit", "Detail")]
        [Description("头像")]
        [Column(StringLength = 36)]
        public string HeadimgUrl { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [OpenApiSubTag("List", "Create", "Edit", "Detail")]
        [Description("姓名")]
        [Column(StringLength = 20)]
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [OpenApiSubTag("List", "Create", "Edit", "Detail")]
        [Description("性别")]
        [Column(StringLength = 2)]
        public string Sex { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [OpenApiSubTag("List", "Create", "Edit", "Detail")]
        [Description("手机号码")]
        [Column(StringLength = 20)]
        public string Tel { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [OpenApiSubTag("List", "Create", "Edit", "Detail")]
        [Description("单位")]
        [Column(StringLength = 20)]
        public string Company { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        [OpenApiSubTag("List", "Create", "Edit", "Detail")]
        [Description("职务")]
        [Column(StringLength = 20)]
        public string Post { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        [OpenApiSubTag("List", "Create", "Edit", "Detail", "Authorities")]
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
        /// 授权给此会员的角色
        /// </summary>
        [Navigate(ManyToMany = typeof(Public_MemberRole))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual ICollection<System_Role> Roles { get; set; }

        /// <summary>
        /// 此会员绑定的微信
        /// </summary>
        [Navigate(ManyToMany = typeof(Public_MemberWeChatUserInfo))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual ICollection<Common_WeChatUserInfo> WeChatUserInfos { get; set; }

        #endregion
    }
}
