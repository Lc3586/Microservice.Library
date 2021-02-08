using Entity.Public;
using Entity.System;
using FreeSql.DataAnnotations;
using Library.Json.Converters;
using Library.OpenApi.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Entity.Common
{
    /// <summary>
    /// 操作记录
    /// </summary>
    [Table]
    [OraclePrimaryKeyName("pk_SPAA_" + nameof(Common_OperationRecord) + "_01")]
    #region 设置索引
    [Index(nameof(Common_OperationRecord) + "_idx_" + nameof(DataId), nameof(DataId) + " ASC")]
    [Index(nameof(Common_OperationRecord) + "_idx_" + nameof(DataType), nameof(DataType) + " ASC")]
    [Index(nameof(Common_OperationRecord) + "_idx_" + nameof(Account), nameof(Account) + " ASC")]
    [Index(nameof(Common_OperationRecord) + "_idx_" + nameof(CreatorId), nameof(CreatorId) + " ASC")]
    [Index(nameof(Common_OperationRecord) + "_idx_" + nameof(CreateTime), nameof(CreateTime) + " DESC")]
    #endregion
    public class Common_OperationRecord
    {
        /// <summary>
        /// Id
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Column(IsPrimary = true, StringLength = 36)]
        public string Id { get; set; }

        /// <summary>
        /// 数据Id
        /// </summary>
        [OpenApiSubTag("_List")]
        [Description("数据Id")]
        [Column(IsNullable = true)]
        public string DataId { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        /// <remarks>值为各数据模型的实体类名</remarks>
        [OpenApiSubTag("List", "Detail")]
        [Description("数据类型")]
        [Column(StringLength = 30)]
        public string DataType { get; set; }

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
        /// <remarks><see cref="Model.System.UserType"/></remarks>
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
        /// 说明
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Description("说明")]
        [Column(StringLength = 256)]
        public string Explain { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [OpenApiSubTag("Detail")]
        [Description("备注")]
        [Column(StringLength = -1)]
        public string Remark { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        [Column(StringLength = 36)]
        public string CreatorId { get; set; }

        /// <summary>
        /// 操作者名称
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [Description("操作者")]
        [Column(StringLength = 50)]
        public string CreatorName { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [OpenApiSubTag("List", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [JsonConverter(typeof(Library.Json.Converters.DateTimeConverter), "yyyy-MM-dd HH:mm:ss")]
        [Description("操作时间")]
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
