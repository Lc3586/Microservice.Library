using Senparc.Weixin.TenPay.V3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 统一下单返回数据
    /// </summary>
    public class WeChatUnifiedorderResult
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr { get; set; }

        /// <summary>
        /// 预支付信息
        /// </summary>
        public string Package { get; set; }

        /// <summary>
        /// TradeType为NATIVE,<see cref="TradeType.NATIVE"/>时有返回，
        /// 此参数可直接生成二维码展示出来进行扫码支付
        /// </summary>
        public string CodeUrl { get; set; }

        /// <summary>
        /// Js支付签名
        /// </summary>
        public string JsPaySign { get; set; }

        /// <summary>
        /// XML内容
        /// </summary>
        public string ResultXml { get; set; }

        /// <summary>
        /// 微信支付分配的终端设备号
        /// </summary>
        public string DeviceInfo { get; set; }

        /// <summary>
        /// 交易类型
        /// <see cref="Model.TradeType"/>
        /// </summary>
        public string TradeType { get; set; }

        /// <summary>
        /// 微信生成的预支付ID，用于后续接口调用中使用
        /// </summary>
        public string PrepayId { get; set; }

        /// <summary>
        /// 在H5支付时返回
        /// </summary>
        public string MWebUrl { get; set; }
    }
}
