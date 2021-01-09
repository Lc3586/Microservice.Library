using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Model
{
    /// <summary>
    /// 统一下单返回数据
    /// </summary>
    public class UnifiedorderResult
    {
        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string AppId { get; set; }

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
        /// 支付签名
        /// </summary>
        public string PaySign { get; set; }

        /// <summary>
        /// 二维码
        /// </summary>
        public string CodeUrl { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public string Data0 { get; set; }
    }
}
