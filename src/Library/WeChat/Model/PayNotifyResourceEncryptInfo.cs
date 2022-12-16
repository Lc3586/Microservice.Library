using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 微信支付通知加密数据
    /// </summary>
    public class PayNotifyResourceEncryptInfo
    {
        /// <summary>
        /// 加密算法类型
        /// </summary>
        /// <remarks>
        /// 对开启结果数据进行加密的加密算法，
        /// 目前只支持AEAD_AES_256_GCM
        /// </remarks>
        public string algorithm { get; set; }

        /// <summary>
        /// 数据密文
        /// </summary>
        /// <remarks>
        /// Base64编码后的开启/停用结果数据密文
        /// </remarks>
        public string ciphertext { get; set; }

        /// <summary>
        /// 数据明文
        /// </summary>
        public PayNotifyResourceInfo Data { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public string associated_data { get; set; }

        /// <summary>
        /// 原始类型
        /// </summary>
        /// <remarks>
        /// 原始回调类型，为transaction 
        /// </remarks>
        public string original_type { get; set; }

        /// <summary>
        /// 随机串
        /// </summary>
        public string nonce { get; set; }
    }
}
