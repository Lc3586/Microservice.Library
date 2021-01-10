using Library.WeChat.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Application
{
    /// <summary>
    /// 微信财付通通知异常
    /// </summary>
    public class WeChatNotifyException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notifyType"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public WeChatNotifyException(WeChatNotifyType notifyType, string message, Exception ex = null)
            : base(message, ex)
        {
            NotifyType = notifyType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notifyType"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public WeChatNotifyException(WeChatNotifyType notifyType, string title, string message, Exception ex = null)
            : base($"{title} : {message}", ex)
        {
            NotifyType = notifyType;
        }

        /// <summary>
        /// 财付通通知类型
        /// </summary>
        public WeChatNotifyType NotifyType { get; }
    }
}
