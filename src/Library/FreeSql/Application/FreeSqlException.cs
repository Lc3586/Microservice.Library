using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.FreeSql.Application
{
    /// <summary>
    /// FreeSql异常
    /// </summary>
    public class FreeSqlException : ApplicationException
    {
        public FreeSqlException(Exception ex)
            : base(null, ex)
        {

        }

        public FreeSqlException(string message, Exception ex = null)
            : base(message, ex)
        {

        }

        public FreeSqlException(string title, string message, Exception ex = null)
            : base($"{title} : {message}", ex)
        {

        }
    }
}
