
namespace Model.System
{
    /// <summary>
    /// Soap配置
    /// </summary>
    public class SoapSetting
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Soap类型
        /// </summary>
        public SoapType Type { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 资源位置
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// 服务接口类
        /// </summary>
        public string ServiceType { get; set; }

        /// <summary>
        /// 服务实现类
        /// </summary>
        public string ImplementationType { get; set; }

        /// <summary>
        /// 序列化类型
        /// </summary>
        public SoapSerializer Serializer { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 自定义响应
        /// </summary>
        public string CustomResponse { get; set; }
    }
}
