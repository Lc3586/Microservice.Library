using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Soap.Application
{
    /// <summary>
    /// Soap客户端配置
    /// </summary>
    public class SoapClientOptions
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 资源位置
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// 服务接口类
        /// Assembly 命名空间
        /// Type 类型名称
        /// </summary>
        public (string Assembly, string Type) ServiceType { get; set; }
    }
}
