using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 单品列表信息
    /// </summary>
    public class GoodsDetailInfo
    {
        /// <summary>
        /// 商品编码
        /// </summary>
        public string goods_id { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        /// <remarks>用户购买的数量 </remarks>
        public int quantity { get; set; }

        /// <summary>
        /// 商品单价
        /// <para>单位为分</para>
        /// </summary>
        public int unit_price { get; set; }

        /// <summary>
        /// 商品优惠金额
        /// <para>单位为分</para>
        /// </summary>
        public int discount_amount { get; set; }

        /// <summary>
        /// 商品备注
        /// </summary>
        public string goods_remark { get; set; }
    }
}
