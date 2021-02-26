using Microservice.Library.WeChat.Model;
using System;

namespace Microservice.Library.WeChat.Extension
{
    /// <summary>
    /// 微信服务财付通通知中间件异常
    /// </summary>
    public class WeChatNotifyException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public WeChatNotifyException(WeChatPayApiVersion version, string message, Exception ex = null)
            : base(message, ex)
        {
            Version = version;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public WeChatNotifyException(WeChatPayApiVersion version, string title, string message, Exception ex = null)
            : base($"{title} : {message}", ex)
        {
            Version = version;
        }

        /// <summary>
        /// 微信支付接口版本
        /// </summary>
        public WeChatPayApiVersion Version { get; }
    }
}
