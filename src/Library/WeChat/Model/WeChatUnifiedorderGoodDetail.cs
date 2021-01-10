using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Model
{
    /// <summary>
    /// 商品明细
    /// </summary>
    public class WeChatUnifiedorderGoodDetail
    {
        #region 必填

        /// <summary>
        /// 商品编码
        /// String(32)
        /// </summary>
        public string goods_id { get; set; }

        /// <summary>
        /// 商品数量
        /// Int(32)
        /// </summary>
        public int quantity { get; set; }

        /// <summary>
        /// 商品单价
        /// Int(32)
        /// <para>单位为分</para>
        /// </summary>
        public int price { get; set; }

        #endregion

        #region 可选

        /// <summary>
        /// 微信支付定义的统一商品编号
        /// String(32)
        /// </summary>
        public string wxpay_goods_id { get; set; }

        /// <summary>
        /// 商品名称
        /// String(256)
        /// </summary>
        public string goods_name { get; set; }

        #endregion
    }
}
