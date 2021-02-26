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
    /// 用户授权资源
    /// </summary>
    [Table]
    public class System_UserResources
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Column(IsPrimary = true, StringLength = 36)]
        public string UserId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        [Column(IsPrimary = true, StringLength = 36)]
        public string ResourcesId { get; set; }

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
