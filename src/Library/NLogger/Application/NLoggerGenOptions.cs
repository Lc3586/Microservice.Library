using System.Collections.Generic;

namespace Microservice.Library.NLogger.Application
{
    /// <summary>
    /// 生成配置
    /// </summary>
    public class NLoggerGenOptions
    {
        /// <summary>
        /// 日志组件目标生成配置
        /// </summary>
        public List<TargetGeneratorOptions> TargetGeneratorOptions { get; set; } = new List<TargetGeneratorOptions>();
    }
}
