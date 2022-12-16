using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 微信签名信息
    /// </summary>
    public class WeChatSignatureInfo
    {
        /// <summary>
        /// 票据
        /// </summary>
        public string Ticket { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr { get; set; }


        /// <summary>
        /// JS-SDK权限验证的签名
        /// </summary>
        public string Signature { get; set; }
    }
}
