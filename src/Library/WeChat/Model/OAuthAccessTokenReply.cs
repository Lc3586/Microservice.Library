using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 网页授权access_token接口返回数据
    /// </summary>
    public class OAuthAccessTokenReply
    {
        /// <summary>
        /// 网页授权接口调用凭证
        /// </summary>
        /// <remarks>注意：此access_token与基础支持的access_token不同</remarks>
        public string access_token { get; set; }

        /// <summary>
        /// access_token接口调用凭证超时时间
        /// </summary>
        /// <remarks>单位（秒）</remarks>
        public string expires_in { get; set; }

        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string refresh_token { get; set; }

        /// <summary>
        /// 用户唯一标识
        /// </summary>
        /// <remarks>请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID</remarks>
        public string openid { get; set; }

        /// <summary>
        /// 用户授权的作用域
        /// </summary>
        /// <remarks>使用逗号（,）分隔</remarks>
        public string scope { get; set; }
    }
}
