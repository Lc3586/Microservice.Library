using Entity.Public;
using Entity.System;
using FreeSql.DataAnnotations;
using Library.OpenApi.Annotations;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Entity.Common
{
    /// <summary>
    /// 登录日志
    /// </summary>
    [Table]
    [OraclePrimaryKeyName("pk_" + nameof(Common_EntryLog))]
    #region 设置索引
    [Index(nameof(Common_EntryLog) + "_idx_" + nameof(Account), nameof(Account) + " ASC")]
    [Index(nameof(Common_EntryLog) + "_idx_" + nameof(UserType), nameof(UserType) + " ASC")]
    [Index(nameof(Common_EntryLog) + "_idx_" + nameof(IsAdmin), nameof(IsAdmin) + " DESC")]
    [Index(nameof(Common_EntryLog) + "_idx_" + nameof(Name), nameof(Name) + " ASC")]
    [Index(nameof(Common_EntryLog) + "_idx_" + nameof(CreatorId), nameof(CreatorId) + " ASC")]
    [Index(nameof(Common_EntryLog) + "_idx_" + nameof(CreateTime), nameof(CreateTime) + " DESC")]
    #endregion
    public class Common_EntryLog
    {
        /// <summary>
        /// Id
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Column(IsPrimary = true, StringLength = 36)]
        public string Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [OpenApiSubTag("List", "Create", "Detail")]
        [Description("账号")]
        [Column(StringLength = 50)]
        public string Account { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        [OpenApiSubTag("List", "Create", "Detail")]
        [Description("用户类型")]
        [Column(StringLength = 20)]
        public string UserType { get; set; }

        /// <summary>
        /// 管理员
        /// </summary>
        [OpenApiSubTag("List", "Create", "Detail")]
        [Description("管理员")]
        [Column(IsNullable = true)]
        public bool? IsAdmin { get; set; }

        /// <summary>
        /// 姓名/昵称
        /// </summary>
        [OpenApiSubTag("List", "Create", "Detail")]
        [Description("姓名/昵称")]
        [Column(StringLength = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [OpenApiSubTag("List", "Create", "Detail")]
        [Description("头像")]
        [Column(StringLength = 36)]
        public string HeadimgUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [OpenApiSubTag("Create", "Detail")]
        [Description("备注")]
        [Column(StringLength = -2)]
        public string Remark { get; set; }

        /// <summary>
        /// 登录人
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Description("登录人")]
        [Column(StringLength = 36)]
        public string CreatorId { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(Library.Json.Converters.DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        [Description("登录时间")]
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

        /// <summary>
        /// 会员
        /// </summary>
        [Navigate(nameof(CreatorId))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual Public_Member Member { get; set; }

        #endregion
    }
}
