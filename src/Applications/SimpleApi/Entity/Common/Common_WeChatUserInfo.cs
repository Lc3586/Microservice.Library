using Entity.Public;
using Entity.System;
using FreeSql.DataAnnotations;
using Microservice.Library.OpenApi.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using DateTimeConverter = Microservice.Library.Json.Converters.DateTimeConverter;

namespace Entity.Common
{
    /// <summary>
    /// 微信用户信息
    /// </summary>
    [Table]
    [OraclePrimaryKeyName("pk_SPAA_" + nameof(Common_FileChunk) + "_01")]
    [Index(nameof(Common_FileChunk) + "_idx_" + nameof(AppId), nameof(AppId) + " ASC")]
    [Index(nameof(Common_FileChunk) + "_idx_" + nameof(OpenId), nameof(OpenId) + " ASC")]
    [Index(nameof(Common_FileChunk) + "_idx_" + nameof(UnionId), nameof(UnionId) + " ASC")]
    [Index(nameof(Common_FileChunk) + "_idx_" + nameof(GroupId), nameof(GroupId) + " ASC")]
    [Index(nameof(Common_FileChunk) + "_idx_" + nameof(Nickname), nameof(Nickname) + " ASC")]
    [Index(nameof(Common_FileChunk) + "_idx_" + nameof(Sex), nameof(Sex) + " ASC")]
    [Index(nameof(Common_FileChunk) + "_idx_" + nameof(Language), nameof(Language) + " ASC")]
    [Index(nameof(Common_FileChunk) + "_idx_" + nameof(City), nameof(City) + " ASC")]
    [Index(nameof(Common_FileChunk) + "_idx_" + nameof(Province), nameof(Province) + " ASC")]
    [Index(nameof(Common_FileChunk) + "_idx_" + nameof(Country), nameof(Country) + " ASC")]
    [Index(nameof(Common_FileChunk) + "_idx_" + nameof(Enable), nameof(Enable) + " ASC")]
    public class Common_WeChatUserInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        [OpenApiSubTag("List")]
        [Column(IsPrimary = true, StringLength = 36)]
        public string Id { get; set; }

        /// <summary>
        /// 微信公众号标识
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Description("微信公众号标识")]
        [Column(StringLength = 36)]
        public string AppId { get; set; }

        /// <summary>
        /// 用户公众号唯一标识
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Description("用户公众号唯一标识")]
        [Column(StringLength = 36)]
        public string OpenId { get; set; }

        /// <summary>
        /// 用户公众平台唯一标识
        /// </summary>
        /// <remarks>只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。</remarks>
        [OpenApiSubTag("List", "Detail")]
        [Description("用户公众平台唯一标识")]
        [Column(StringLength = 36)]
        public string UnionId { get; set; }

        /// <summary>
        /// 分组标识
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Description("分组标识")]
        [Column(StringLength = 36)]
        public string GroupId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Description("昵称")]
        [Column(StringLength = 50)]
        public string Nickname { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Description("头像")]
        [Column(StringLength = 2048)]
        public string HeadimgUrl { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Description("性别")]
        public byte Sex { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Description("城市")]
        [Column(StringLength = 20)]
        public string City { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Description("省份")]
        [Column(StringLength = 20)]
        public string Province { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Description("国家")]
        [Column(StringLength = 20)]
        public string Country { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Description("语言")]
        [Column(StringLength = 10)]
        public string Language { get; set; }

        /// <summary>
        /// 用户授权的作用域
        /// </summary>
        /// <remarks>逗号[,]分隔</remarks>
        [OpenApiSubTag("List", "Detail")]
        [Description("用户授权的作用域")]
        [Column(StringLength = 100)]
        public string Scope { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Description("启用")]
        public bool Enable { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Description("备注")]
        [Column(StringLength = -2)]
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [OpenApiSubTag("List", "Detail", "FileInfo")]
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近更新时间
        /// </summary>
        [OpenApiSubTag("List", "Detail", "_Edit")]
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        [Description("最近更新时间")]
        [Column(IsNullable = true)]
        public DateTime? ModifyTime { get; set; }

        #region 关联

        /// <summary>
        /// 绑定此微信的用户
        /// </summary>
        [Navigate(ManyToMany = typeof(System_UserWeChatUserInfo))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual ICollection<System_User> Users { get; set; }

        /// <summary>
        /// 绑定此微信的会员
        /// </summary>
        [Navigate(ManyToMany = typeof(Public_MemberWeChatUserInfo))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual ICollection<Public_Member> Members { get; set; }

        #endregion
    }
}
