using Entity.Common;
using FreeSql.DataAnnotations;
using Library.OpenApi.Annotations;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Entity.System
{
    /// <summary>
    /// 用户绑定的微信
    /// </summary>
    [Table]
    public class System_UserWeChatUserInfo
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Column(IsPrimary = true, StringLength = 36)]
        public string UserId { get; set; }

        /// <summary>
        /// 微信用户信息Id
        /// </summary>
        [Column(IsPrimary = true, StringLength = 36)]
        public string WeChatUserInfoId { get; set; }

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
        /// 微信用户信息
        /// </summary>
        [Navigate(nameof(WeChatUserInfoId))]
        [OpenApiIgnore]
        [JsonIgnore]
        [XmlIgnore]
        public virtual Common_WeChatUserInfo WeChatUserInfo { get; set; }

        #endregion
    }
}
