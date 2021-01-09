using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Model
{
    /// <summary>
    /// 微信支付通知数据
    /// </summary>
    public class PayNotifyResourceInfo
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string mchid { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public string trade_type { get; set; }

        /// <summary>
        /// 交易状态
        /// </summary>
        public string trade_state { get; set; }

        /// <summary>
        /// 交易状态描述
        /// </summary>
        public string trade_state_desc { get; set; }

        /// <summary>
        /// 付款银行
        /// </summary>
        public string bank_type { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        /// <remarks>
        /// 附加数据，
        /// 在查询API和支付通知中原样返回，
        /// 可作为自定义参数使用
        /// </remarks>
        public string attach { get; set; }

        /// <summary>
        /// 支付完成时间
        /// </summary>
        /// <remarks>
        /// 遵循rfc3339标准格式，
        /// 格式为YYYY-MM-DDTHH:mm:ss+TIMEZONE，
        /// YYYY-MM-DD表示年月日，T出现在字符串中，表示time元素的开头，
        /// HH:mm:ss.表示时分秒，TIMEZONE表示时区（+08:00表示东八区时间，领先UTC 8小时，即北京时间）。
        /// 例如：2015-05-20T13:29:35+08:00表示北京时间2015年05月20日13点29分35秒。
        /// </remarks>
        public string success_time { get; set; }

        /// <summary>
        /// 支付者
        /// </summary>
        public PayerInfo payer { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public AmountInfo amount { get; set; }

        /// <summary>
        /// 场景信息
        /// </summary>
        public SceneInfo scene_info { get; set; }

        /// <summary>
        /// 优惠功能
        /// </summary>
        public PromotionDetailInfo promotion_detail { get; set; }
    }
}
