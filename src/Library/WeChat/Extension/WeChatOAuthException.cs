using Microservice.Library.WeChat.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.WeChat.Extension
{
    /// <summary>
    /// 微信网页授权中间件异常
    /// </summary>
    public class WeChatOAuthException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public WeChatOAuthException(OAuthVersion version, string message, Exception ex = null)
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
        public WeChatOAuthException(OAuthVersion version, string title, string message, Exception ex = null)
            : base($"{title} : {message}", ex)
        {
            Version = version;
        }

        /// <summary>
        /// 财付通通知类型
        /// </summary>
        public OAuthVersion Version { get; }
    }
}
