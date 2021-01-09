using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Application
{
    /// <summary>
    /// 微信服务异常
    /// </summary>
    internal class WeChatServiceException : ApplicationException
    {
        public WeChatServiceException(string message, Exception ex = null)
            : base(message, ex)
        {

        }

        public WeChatServiceException(string title, string message, Exception ex = null)
            : base($"{title} : {message}", ex)
        {

        }
    }
}
