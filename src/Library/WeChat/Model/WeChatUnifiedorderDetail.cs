using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Model
{
    /// <summary>
    /// 商品详细列表
    /// </summary>
    public class WeChatUnifiedorderDetail
    {
        #region 服务商必填

        /// <summary>
        /// 商品明细
        /// </summary>
        public IList<WeChatUnifiedorderGoodDetail> goods_detail { get; set; }

        #endregion

        #region 可选

        /// <summary>
        /// 商户侧一张小票订单可能被分多次支付，
        /// 订单原价用于记录整张小票的支付金额。
        /// 当订单原价与支付金额不相等则被判定为拆单，无法享/受/优/惠。
        /// Int(32)
        /// <para>单位为分</para>
        /// </summary>
        public int cost_price { get; set; }

        /// <summary>
        /// 商家小票ID 
        /// String(32)
        /// </summary>
        public string receipt_id { get; set; }

        #endregion
    }
}
