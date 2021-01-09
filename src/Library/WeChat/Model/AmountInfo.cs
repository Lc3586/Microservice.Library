using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Model
{
    /// <summary>
    /// 订单金额信息
    /// </summary>
    public class AmountInfo
    {
        /// <summary>
        /// 总金额
        /// <para>单位为分</para>
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 用户支付金额
        /// <para>单位为分</para>
        /// </summary>
        public int payer_total { get; set; }

        /// <summary>
        /// 货币类型
        /// </summary>
        /// <remarks>
        /// 境内商户号仅支持人民币。
        /// </remarks>
        public string currency { get; set; }

        /// <summary>
        /// 用户支付币种
        /// </summary>
        public string payer_currency { get; set; }
    }
}
