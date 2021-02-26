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
    /// 角色授权资源
    /// </summary>
    [Table]
    public class System_RoleResources
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [Column(IsPrimary = true, StringLength = 36)]
        public string RoleId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        [Column(IsPrimary = true, StringLength = 36)]
        public string ResourcesId { get; set; }

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
        /// 资源
        /// </summary>
        [Navigate(nameof(ResourcesId))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual System_Resources Resources { get; set; }

        #endregion
    }
}
