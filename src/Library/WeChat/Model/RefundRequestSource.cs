using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 退款发起来源
    /// </summary>
    public static class RefundRequestSource
    {
        /// <summary>
        /// 接口
        /// </summary>
        [Description("接口")]
        public const string API = "API";

        /// <summary>
        /// 商户平台
        /// </summary>
        [Description("商户平台")]
        public const string VENDOR_PLATFORM = "VENDOR_PLATFORM";
    }
}
