using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.NLogger.Gen
{
    /// <summary>
    /// 日志组件构造器
    /// </summary>
    public interface INLoggerProvider
    {
        /// <summary>
        /// 获取日志组件
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        NLog.ILogger GetNLogger(string name);
    }
}
