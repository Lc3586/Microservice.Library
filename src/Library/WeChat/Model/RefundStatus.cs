using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Model
{
    /// <summary>
    /// 退款状态
    /// </summary>
   public static class RefundStatus
    {
        /// <summary>
        /// 退款成功
        /// </summary>
        public const string SUCCESS = "SUCCESS";

        /// <summary>
        /// 退款关闭
        /// </summary>
        public const string REFUNDCLOSE = "REFUNDCLOSE";

        /// <summary>
        /// 退款异常，退款到银行发现用户的卡作废或者冻结了，导致原路退款银行卡失败，可前往商户平台（pay.weixin.qq.com）-交易中心，手动处理此笔退款。 
        /// </summary>
        public const string CHANGE = "CHANGE";
    }
}
