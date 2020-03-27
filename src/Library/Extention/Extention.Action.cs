using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Extention
{
    /// <summary>
    /// 拓展类
    /// </summary>
    public static partial class Extention
    {
        /// <summary>
        /// 循环指定次数
        /// </summary>
        /// <param name="count">循环次数</param>
        /// <param name="method">执行的方法</param>
        public static void Loop(this Action method, int count)
        {
            for (int i = 0; i < count; i++)
            {
                method();
            }
        }

        /// <summary>
        /// 循环指定次数
        /// </summary>
        /// <param name="count">循环次数</param>
        /// <param name="method">执行的方法</param>
        public static void Loop(this Action<int> method, int count)
        {
            for (int i = 0; i < count; i++)
            {
                method(i);
            }
        }
    }
}
