using FreeSql.DataAnnotations;
using Library.OpenApi.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Entity.System
{
    /// <summary>
    /// 角色授权菜单
    /// </summary>
    [Table]
    public class System_RoleMenu
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [Column(IsPrimary = true, StringLength = 36)]
        public string RoleId { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        [Column(IsPrimary = true, StringLength = 36)]
        public string MenuId { get; set; }

        #region 关联

        /// <summary>
        /// 角色
        /// </summary>
        [Navigate(nameof(RoleId))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual System_Role Role { get; set; }

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
