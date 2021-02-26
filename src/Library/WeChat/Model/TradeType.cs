using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 交易类型
    /// </summary>
    public static class TradeType
    {
        /// <summary>
        /// 公众号支付 
        /// </summary>
        [Description("公众号支付")]
        public const string JSAPI = "JSAPI";

        /// <summary>
        /// 扫码支付 
        /// </summary>
        [Description("扫码支付")]
        public const string NATIVE = "NATIVE";

        /// <summary>
        /// APP支付 
        /// </summary>
        [Description("APP支付")]
        public const string APP = "APP";

        /// <summary>
        /// 付款码支付 
        /// </summary>
        [Description("付款码支付")]
        public const string MICROPAY = "MICROPAY";

        /// <summary>
        /// H5支付
        /// </summary>
        [Description("H5支付")]
        public const string MWEB = "MWEB";

        /// <summary>
        /// 刷脸支付 
        /// </summary>
        [Description("刷脸支付")]
        public const string FACEPAY = "FACEPAY";
    }
}
