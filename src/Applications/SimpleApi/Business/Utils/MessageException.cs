using Model.System;
using System;

namespace Business.Utils
{
    /// <summary>
    /// 消息异常
    /// </summary>
    public class MessageException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="code">错误代码</param>
        /// <param name="innerException">内部异常</param>
        public MessageException(string msg, ErrorCode code = ErrorCode.error, Exception innerException = null)
        {
            Msg = msg;
            Code = code;
            if (innerException != null)
                InnerException = innerException;
        }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public ErrorCode Code { get; set; }

        /// <summary>
        /// 内部异常
        /// </summary>
        public new Exception InnerException { get; set; }
    }

    /// <summary>
    /// 验证异常
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="innerException">内部异常</param>
        public ValidationException(string msg, Exception innerException = null)
        {
            Msg = msg;
            if (innerException != null)
                InnerException = innerException;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="innerException">内部异常</param>
        public ValidationException(object data, Exception innerException = null)
        {
            Data = data;
            if (innerException != null)
                InnerException = innerException;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="data">数据</param>
        /// <param name="innerException">内部异常</param>
        public ValidationException(string msg, object data, Exception innerException = null)
        {
            Msg = msg;
            Data = data;
            if (innerException != null)
                InnerException = innerException;
        }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public new object Data { get; set; }

        /// <summary>
        /// 内部异常
        /// </summary>
        public new Exception InnerException { get; set; }
    }
}
