using System;
using System.Collections.Generic;
using System.Text;

namespace T4CAGC.Models
{
    /// <summary>
    /// 程序状态
    /// </summary>
    public enum ProgramState
    {
        /// <summary>
        /// 待机
        /// </summary>
        Standby,
        /// <summary>
        /// 运行
        /// </summary>
        Run,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 退出
        /// </summary>
        Exit
    }
}
