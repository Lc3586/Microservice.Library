using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 微信通知数据加密算法类型
    /// </summary>
    public static class AlgorithmType
    {
        /// <summary>
        /// AEAD_AES_256_GCM
        /// </summary>
        [Description("AEAD_AES_256_GCM")]
        public const string AEAD_AES_256_GCM = "AEAD_AES_256_GCM";
    }
}
