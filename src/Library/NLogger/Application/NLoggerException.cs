using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.NLogger.Application
{
    /// <summary>
    /// NLogger异常
    /// </summary>
    public class NLoggerException : ApplicationException
    {
        public NLoggerException(Exception ex)
            : base(null, ex)
        {

        }

        public NLoggerException(string message, Exception ex = null)
            : base(message, ex)
        {

        }

        public NLoggerException(string title, string message, Exception ex = null)
            : base($"{title} : {message}", ex)
        {

        }
    }
}
