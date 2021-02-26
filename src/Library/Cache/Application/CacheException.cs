using System;

namespace Microservice.Library.Cache.Application
{
    /// <summary>
    /// 缓存异常
    /// </summary>
    public class CacheException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public CacheException(string message, Exception ex = null)
            : base(message, ex)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public CacheException(string title, string message, Exception ex = null)
            : base($"{title} : {message}", ex)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <param name="dataType"></param>
        /// <param name="ex"></param>
        public CacheException(string message, object data, Type dataType, Exception ex = null)
            : base(message, ex)
        {
            ResultData = data;
            DataType = dataType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// '<param name="dataType"></param>
        /// <param name="ex"></param>
        public CacheException(string title, string message, object data, Type dataType, Exception ex = null)
            : base($"{title} : {message}", ex)
        {
            ResultData = data;
            DataType = dataType;
        }

        /// <summary>
        /// 数据
        /// </summary>

        public object ResultData { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public Type DataType { get; set; }
    }
}
