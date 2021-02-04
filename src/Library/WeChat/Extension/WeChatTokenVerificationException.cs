using Library.WeChat.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Extension
{
    /// <summary>
    /// 微信开发令牌验证中间件异常
    /// </summary>
    public class WeChatTokenVerificationException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public WeChatTokenVerificationException(string message, Exception ex = null)
            : base(message, ex)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public WeChatTokenVerificationException(string title, string message, Exception ex = null)
            : base($"{title} : {message}", ex)
        {

        }
    }
}
