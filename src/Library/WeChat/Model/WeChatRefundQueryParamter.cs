using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Model
{
    /// <summary>
    /// 微信查询退款参数
    /// </summary>
    public class WeChatRefundQueryParamter
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="outTradeNo">商家订单号</param>
        /// <param name="outRefundNo">商户侧传给微信的退款单号</param>
        /// <param name="refundId">
        /// 微信生成的退款单号，
        /// 在申请退款接口有返回
        /// </param>
        /// <param name="offset">
        /// 偏移量，
        /// 当部分退款次数超过10次时可使用，
        /// 表示返回的查询结果从这个偏移量开始取记录，如：15
        /// </param>
        /// <returns></returns>
        public static WeChatRefundQueryParamter GetSimpleParamter(string outTradeNo, string outRefundNo, string refundId, int? offset = null)
        {
            return new WeChatRefundQueryParamter
            {
                OutTradeNo = outTradeNo,
                OutRefundNo = outRefundNo,
                RefundId = refundId,
                Offset = offset
            };
        }

        #region 必填

        /// <summary>
        /// 商户系统内部的订单号
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 商户侧传给微信的退款单号
        /// </summary>
        public string OutRefundNo { get; set; }

        /// <summary>
        /// 微信生成的退款单号，
        /// 在申请退款接口有返回
        /// </summary>
        public string RefundId { get; set; }

        #endregion

        #region 选填

        /// <summary>
        /// 微信订单号
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
        /// 偏移量，
        /// 当部分退款次数超过10次时可使用，
        /// 表示返回的查询结果从这个偏移量开始取记录，如：15
        /// </summary>
        public int? Offset { get; set; }

        #endregion
    }
}
