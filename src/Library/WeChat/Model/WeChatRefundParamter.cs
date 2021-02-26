using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 微信退款参数
    /// </summary>
    public class WeChatRefundParamter
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="outTradeNo">商家订单号</param>
        /// <param name="outRefundNo">商户侧传给微信的退款单号</param>
        /// <param name="totalFee">
        /// 订单金额
        /// 订单总金额，单位为分，只能为整数，详见支付金额
        /// </param>
        /// <param name="refundFee">
        /// 退款金额
        /// 退款总金额，订单总金额，单位为分，只能为整数，详见支付金额
        /// </param>
        /// <returns></returns>
        public static WeChatRefundParamter GetSimpleParamter(string outTradeNo, string outRefundNo, int totalFee, int refundFee)
        {
            return new WeChatRefundParamter
            {
                OutTradeNo = outTradeNo,
                OutRefundNo = outRefundNo,
                TotalFee = totalFee,
                RefundFee = refundFee
            };
        }

        #region 必填

        /// <summary>
        /// 商户系统内部的订单号（和TransactionId二选一）
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 商户侧传给微信的退款单号
        /// </summary>
        public string OutRefundNo { get; set; }

        /// <summary>
        /// 订单金额。
        /// 订单总金额，单位为分，只能为整数，详见支付金额
        /// </summary>
        public int TotalFee { get; set; }

        /// <summary>
        /// 退款金额。
        /// 退款总金额，订单总金额，单位为分，只能为整数，详见支付金额
        /// </summary>
        public int RefundFee { get; set; }

        #endregion

        #region 选填

        /// <summary>
        /// 退款资金来源。
        /// <see cref="RefundAccountType"/>
        /// 仅针对老资金流商户使用 REFUND_SOURCE_UNSETTLED_FUNDS---未结算资金退款（默认使用未结算资金退款） REFUND_SOURCE_RECHARGE_FUNDS---可用余额退款(限非当日交易订单的退款）
        /// </summary>
        public string RefundAccount { get; set; } = RefundAccountType.REFUND_SOURCE_UNSETTLED_FUNDS;

        /// <summary>
        /// 若商户传入，会在下发给用户的退款消息中体现退款原因
        /// </summary>
        public string RefundDescription { get; }

        /// <summary>
        /// 货币类型，
        /// <see cref="FreeType"/>
        /// 符合ISO 4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型
        /// </summary>
        public string RefundFeeType { get; set; } = FreeType.CNY;

        /// <summary>
        /// 微信订单号（和OutTradeNo二选一）
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr { get; }

        /// <summary>
        /// 商户自定义的终端设备号，
        /// 如门店编号、设备的ID
        /// </summary>
        public string DeviceInfo { get; set; }

        /// <summary>
        /// 超时时间（毫秒）
        /// </summary>
        public int TimeOut { get; set; } = 10000;

        #endregion
    }
}
