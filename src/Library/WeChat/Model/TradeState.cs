using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 交易状态
    /// </summary>
    public static class TradeState
    {
        /// <summary>
        /// 支付成功 
        /// </summary>
        [Description("支付成功")]
        public const string SUCCESS = "SUCCESS";

        /// <summary>
        /// 转入退款 
        /// </summary>
        [Description("转入退款")]
        public const string REFUND = "REFUND";

        /// <summary>
        /// 未支付 
        /// </summary>
        [Description("未支付")]
        public const string NOTPAY = "NOTPAY";

        /// <summary>
        /// 已关闭 
        /// </summary>
        [Description("已关闭")]
        public const string CLOSED = "CLOSED";

        /// <summary>
        /// 已撤销(付款码支付)
        /// </summary>
        [Description("已撤销(付款码支付)")]
        public const string REVOKED = "REVOKED";

        /// <summary>
        /// 用户支付中(付款码支付)
        /// </summary>
        [Description("用户支付中(付款码支付)")]
        public const string USERPAYING = "USERPAYING";

        /// <summary>
        /// 支付失败(其他原因，如银行返回失败) 
        /// </summary>
        [Description("支付失败(其他原因，如银行返回失败)")]
        public const string PAYERROR = "PAYERROR";
    }
}
