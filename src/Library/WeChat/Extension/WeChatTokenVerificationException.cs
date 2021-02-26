using System;

namespace Microservice.Library.WeChat.Extension
{
    /// <summary>
    /// 微信开发令牌验证中间件异常
    /// </summary>
    public class WeChatTokenVerificationException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        public WeChatTokenVerificationException()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public WeChatTokenVerificationException(string message) : base(message)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public WeChatTokenVerificationException(string message, Exception ex = null)
            : base(message, ex)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public WeChatTokenVerificationException(string title, string message, Exception ex = null)
            : base($"{title} : {message}", ex)
        {

        }
    }
}
