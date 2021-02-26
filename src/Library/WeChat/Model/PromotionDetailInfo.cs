using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 优惠功能
    /// </summary>
    public class PromotionDetailInfo
    {
        /// <summary>
        /// 券ID
        /// </summary>
        public string coupon_id { get; set; }

        /// <summary>
        /// 优惠名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 优惠范围
        /// </summary>
        public string scope { get; set; }

        /// <summary>
        /// 优惠类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 优惠券面额
        /// <para>单位为分</para>
        /// </summary>
        public int amount { get; set; }

        /// <summary>
        /// 活动ID
        /// </summary>
        public string stock_id { get; set; }

        /// <summary>
        /// 微信出资
        /// <para>单位为分</para>
        /// </summary>
        public int wechatpay_contribute { get; set; }

        /// <summary>
        /// 商户出资
        /// <para>单位为分</para>
        /// </summary>
        public int merchant_contribute { get; set; }

        /// <summary>
        /// 其他出资
        /// <para>单位为分</para>
        /// </summary>
        public int other_contribute { get; set; }

        /// <summary>
        /// 优惠币种
        /// </summary>
        /// <remarks>境内商户号仅支持人民币。 </remarks>
        public string currency { get; set; }

        /// <summary>
        /// 单品列表
        /// </summary>
        public List<GoodsDetailInfo> goods_detail { get; set; }
    }
}
