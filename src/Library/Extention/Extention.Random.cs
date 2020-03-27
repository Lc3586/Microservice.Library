using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Extention
{
    /// <summary>
    /// 拓展类
    /// </summary>
    public static partial class Extention
    {
        /// <summary>
        /// 下一个随机值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="source">值的集合</param>
        /// <returns></returns>
        public static T Next<T>(this Random random, IEnumerable<T> source)
        {
            return source.ToList()[random.Next(0, source.Count())];
        }
    }
}
