using System;

namespace Microservice.Library.SampleAuthentication.Extension
{
    /// <summary>
    /// 简易身份验证服务中间件异常
    /// </summary>
    public class SampleAuthenticationException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public SampleAuthenticationException(string message, Exception ex = null)
            : base(message, ex)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public SampleAuthenticationException(string title, string message, Exception ex = null)
            : base($"{title} : {message}", ex)
        {

        }
    }
}
