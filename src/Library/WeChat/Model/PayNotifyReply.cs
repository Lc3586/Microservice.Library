using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Model
{
    /// <summary>
    /// 微信支付通知应答信息
    /// </summary>
    public class PayNotifyReply
    {
        /// <summary>
        /// 返回状态码
        /// </summary>
        /// <see cref="ReturnCode"/>
        /// <remarks>
        /// SUCCESS为清算机构接收成功，
        /// 其他错误码为失败。
        /// </remarks>
        public string code { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        /// <remarks>如非空，为错误原因。 </remarks>
        public string message { get; set; }
    }
}
