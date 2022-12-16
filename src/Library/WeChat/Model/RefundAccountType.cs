using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 退款资金来源
    /// </summary>
    public static class RefundAccountType
    {
        /// <summary>
        /// 可用余额退款/基本账户
        /// </summary>
        [Description("可用余额退款/基本账户")]
        public const string REFUND_SOURCE_RECHARGE_FUNDS = "REFUND_SOURCE_RECHARGE_FUNDS";

        /// <summary>
        /// 未结算资金退款 
        /// </summary>
        [Description("未结算资金退款")]
        public const string REFUND_SOURCE_UNSETTLED_FUNDS = "REFUND_SOURCE_UNSETTLED_FUNDS";
    }
}
