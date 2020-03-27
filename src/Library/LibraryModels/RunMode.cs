using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Models
{
    /// <summary>
    /// 运行模式
    /// </summary>
    public enum RunMode
    {
        /// <summary>
        /// 本地测试模式
        /// </summary>
        LocalTest,

        /// <summary>
        /// 本地测试模式（严格）
        /// </summary>
        LocalTest_Strict,

        /// <summary>
        /// 发布模式
        /// </summary>
        Publish
    }
}
