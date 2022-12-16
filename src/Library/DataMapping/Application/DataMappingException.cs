using System;

namespace Microservice.Library.DataMapping.Application
{
    /// <summary>
    /// 异常
    /// </summary>
    /// <returns></returns>
    public class DataMappingException : InvalidOperationException
    {
        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">内容</param>
        public DataMappingException(string title, string message = null)
            : base($"{title} : {message}")
        {

        }
    }
}
