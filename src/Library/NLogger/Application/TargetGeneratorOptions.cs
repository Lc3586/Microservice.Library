using NLog;
using System.Collections.Generic;

namespace Microservice.Library.NLogger.Application
{
    /// <summary>
    /// 日志组件目标生成配置
    /// </summary>
    public class TargetGeneratorOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public TargetGeneratorOptions()
        {
            MinLevel = LogLevel.Trace;
            MaxLevel = LogLevel.Off;
        }

        /// <summary>
        /// 最低等级
        /// </summary>
        /// <remarks>默认 <see cref="LogLevel.Trace"/></remarks>
        public LogLevel MinLevel { get; set; }

        /// <summary>
        /// 最高等级
        /// </summary>
        /// <remarks>默认 <see cref="LogLevel.Off"/></remarks>
        public LogLevel MaxLevel { get; set; }

        /// <summary>
        /// 自定义日志对象
        /// </summary>
        /// <remarks>不指定时自动创建</remarks>
        public NLog.Targets.Target Target { get; set; }
    }
}
