using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.Soap.Application
{
    /// <summary>
    /// 生成选项
    /// </summary>
    public class SaopClientGeneratorOptions
    {
        public SaopClientGeneratorOptions()
        {
            SoapClients = new List<SoapClientOptions>();
        }

        /// <summary>
        /// Soap客户端
        /// </summary>
        public List<SoapClientOptions> SoapClients { get; set; }
    }
}
