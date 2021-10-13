using Microsoft.AspNetCore.Http;
using SoapCore;
using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Text;

namespace Microservice.Library.Soap.Application
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
        /// 默认值 XmlSerializer
        /// </summary>
        public SoapSerializer Serializer { get; set; } = SoapSerializer.XmlSerializer;

        /// <summary>
        /// 编码器选项
        /// </summary>
        public SoapEncoderOptions[] EncoderOptions { get; set; }

        /// <summary>
        /// 自定义响应
        /// </summary>
        /// <remarks>必须设置HttpContextGetter!</remarks>
        public string CustomResponse { get; set; }

        /// <summary>
        /// Http获取器
        /// </summary>
        public Func<HttpContext> HttpContextGetter { get; set; }

        /// <summary>
        /// 设置SoapEndpoint
        /// </summary>
        public Action<SoapCoreOptions> SetupSoapEndpoint = (options) => { };
    }
}
