using Microsoft.Extensions.Logging;

namespace Microservice.Library.SuperSocket.Model
{
    /// <summary>
    /// 日志配置
    /// </summary>
    public class SuperSocketLoggingOptions
    {
        /// <summary>
        /// 日志构造器
        /// </summary>
        public ILoggerProvider Provider { get; set; }

        /// <summary>
        /// 添加控制台日志
        /// </summary>
        public bool AddConsole { get; set; }

        /// <summary>
        /// 添加调试日志
        /// </summary>
        public bool AddDebug { get; set; }
    }
}
