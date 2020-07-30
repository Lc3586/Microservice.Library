using SoapCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Soap.Application
{
    /// <summary>
    /// Soap服务端配置
    /// </summary>
    public class SoapServerOptions
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 服务接口类
        /// Assembly 命名空间
        /// Type 类型名称
        /// </summary>
        public (string Assembly, string Type) ServiceType { get; set; }

        /// <summary>
        /// 服务实现类
        /// Assembly 命名空间
        /// Type 类型名称
        /// </summary>
        public (string Assembly, string Type) ImplementationType { get; set; }

        /// <summary>
        /// 序列化类型
        /// </summary>
        public SoapSerializer Serializer { get; set; }
    }
}
