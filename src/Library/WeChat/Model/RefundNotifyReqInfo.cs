using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 退款通知解密信息
    /// </summary>
    public class RefundNotifyReqInfo
    {
        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 微信退款单号
        /// </summary>
        public string refund_id { get; set; }

        /// <summary>
        /// 总金额
        /// <para>单位为分</para>
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 应结订单金额
        /// <para>单位为分</para>
        /// </summary>
        /// <remarks>
        /// 当订单使用了免充值型优惠券后返回该参数，
        /// 应结订单金额=订单金额-免充值优惠券金额。
        /// </remarks>
        public int settlement_total_fee { get; set; }

        /// <summary>
        /// 申请退款金额
        /// <para>单位为分</para>
        /// </summary>
        public int refund_fee { get; set; }

        /// <summary>
        /// 退款金额
        /// <para>单位为分</para>
        /// </summary>
        /// <remarks>
        /// 去掉非充值代金券退款金额后的退款金额，
        /// 退款金额=申请退款金额-非充值代金券退款金额，
        /// 退款金额小于等于申请退款金额
        /// </remarks>
        public int settlement_refund_fee { get; set; }

        /// <summary>
        /// 退款状态
        /// </summary>
        public string refund_status { get; set; }

        /// <summary>
        /// 退款入账账户
        /// </summary>
        /// <remarks>
        /// 取当前退款单的退款入账方 
        /// 1）退回银行卡： 
        /// {银行名称}{卡类型}{ 卡尾号}
        /// 2）退回支付用户零钱:
        /// 支付用户零钱
        /// 3）退还商户:
        /// 商户基本账户
        /// 商户结算银行账户
        /// 4）退回支付用户零钱通:
        /// 支付用户零钱通
        /// </remarks>
        public string refund_recv_accout { get; set; }

        /// <summary>
        /// 退款成功时间
        /// </summary>
        public string refund_success_time { get; set; }

        /// <summary>
        /// 退款资金来源
        /// </summary>
        public string refund_account { get; set; }

        /// <summary>
        /// 退款发起来源
        /// </summary>
        public string refund_request_source { get; set; }
    }
}
