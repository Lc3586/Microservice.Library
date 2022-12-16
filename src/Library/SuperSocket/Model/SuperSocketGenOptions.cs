namespace Microservice.Library.SuperSocket.Model
{
    /// <summary>
    /// SuperSocket配置
    /// </summary>
    /// <typeparam name="TPackageInfo">消息包类型</typeparam>
    public class SuperSocketGenOptions<TPackageInfo> where TPackageInfo : class
    {
        /// <summary>
        /// 服务器配置
        /// </summary>
        public SuperSocketServerOptions<TPackageInfo> ServerOptions { get; set; } = new SuperSocketServerOptions<TPackageInfo>();

        /// <summary>
        /// 日志配置
        /// </summary>
        public SuperSocketLoggingOptions LoggingOptions { get; set; } = new SuperSocketLoggingOptions();
    }
}
