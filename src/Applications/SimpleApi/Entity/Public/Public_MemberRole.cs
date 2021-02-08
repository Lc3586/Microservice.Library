using Entity.System;
using FreeSql.DataAnnotations;
using Library.OpenApi.Annotations;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Entity.Public
{
    /// <summary>
    /// 会员授权角色
    /// </summary>
    [Table]
    public class Public_MemberRole
    {
        /// <summary>
        /// 会员Id
        /// </summary>
        [Column(IsPrimary = true, StringLength = 36)]
        public string MemberId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [Column(IsPrimary = true, StringLength = 36)]
        public string RoleId { get; set; }

        #region 关联

        /// <summary>
        /// 会员
        /// </summary>
        [Navigate(nameof(MemberId))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual Public_Member Member { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [Navigate(nameof(RoleId))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual System_Role Role { get; set; }

        #endregion
    }
}
