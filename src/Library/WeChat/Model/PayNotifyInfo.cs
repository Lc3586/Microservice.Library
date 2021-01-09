using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Model
{
    /// <summary>
    /// 微信支付通知
    /// </summary>
    public class PayNotifyInfo
    {
        /// <summary>
        /// 通知ID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 通知创建时间
        /// </summary>
        /// <remarks>
        /// 遵循rfc3339标准格式，
        /// 格式为YYYY-MM-DDTHH:mm:ss+TIMEZONE，
        /// YYYY-MM-DD表示年月日，T出现在字符串中，表示time元素的开头，
        /// HH:mm:ss.表示时分秒，TIMEZONE表示时区（+08:00表示东八区时间，领先UTC 8小时，即北京时间）。
        /// 例如：2015-05-20T13:29:35+08:00表示北京时间2015年05月20日13点29分35秒。
        /// </remarks>
        public string create_time { get; set; }

        /// <summary>
        /// 通知类型
        /// </summary>
        public string event_type { get; set; }

        /// <summary>
        /// 通知数据类型
        /// </summary>
        public string resource_type { get; set; }

        /// <summary>
        /// 加密数据
        /// </summary>
        public PayNotifyResourceEncryptInfo resource { get; set; }

        /// <summary>
        /// 明文数据
        /// </summary>
        public PayNotifyResourceInfo ResourceDecrypt { get; set; }

        /// <summary>
        /// 回调摘要
        /// </summary>
        public string summary { get; set; }
    }
}
