using Entity.Common;
using Entity.Public;
using FreeSql.DataAnnotations;
using Library.OpenApi.Annotations;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Entity.Public
{
    /// <summary>
    /// 会员绑定的微信
    /// </summary>
    [Table]
    public class Public_MemberWeChatUserInfo
    {
        /// <summary>
        /// 会员Id
        /// </summary>
        [Column(IsPrimary = true, StringLength = 36)]
        public string MemberId { get; set; }

        /// <summary>
        /// 微信用户信息Id
        /// </summary>
        [Column(IsPrimary = true, StringLength = 36)]
        public string WeChatUserInfoId { get; set; }

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
