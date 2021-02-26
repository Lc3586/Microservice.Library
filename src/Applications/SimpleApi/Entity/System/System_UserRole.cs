using FreeSql.DataAnnotations;
using Microservice.Library.OpenApi.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Entity.System
{
    /// <summary>
    /// 用户授权角色
    /// </summary>
    [Table]
    public class System_UserRole
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Column(IsPrimary = true, StringLength = 36)]
        public string UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [Column(IsPrimary = true, StringLength = 36)]
        public string RoleId { get; set; }

        #region 关联

        /// <summary>
        /// 用户
        /// </summary>
        [Navigate(nameof(UserId))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual System_User User { get; set; }

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
