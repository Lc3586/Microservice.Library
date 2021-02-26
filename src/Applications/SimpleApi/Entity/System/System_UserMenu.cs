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
    /// 用户授权菜单
    /// </summary>
    [Table]
    public class System_UserMenu
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Column(IsPrimary = true, StringLength = 36)]
        public string UserId { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        [Column(IsPrimary = true, StringLength = 36)]
        public string MenuId { get; set; }

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
        /// 菜单
        /// </summary>
        [Navigate(nameof(MenuId))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual System_Menu Menu { get; set; }

        #endregion
    }
}
