using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Soap.Application
{
    /// <summary>
    /// Soap异常
    /// </summary>
    public class SoapException : ApplicationException
    {
        public SoapException(string message)
            : base(message)
        {

        }

        public SoapException(string title, string message = null)
            : base($"{title} : {message}")
        {

        }
    }
}
