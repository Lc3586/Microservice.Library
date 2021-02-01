using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Utils
{
    /// <summary>
    /// 雪花Id帮助类
    /// </summary>
    public static class IdHelper
    {
        /// <summary>
        /// 下一个Id
        /// </summary>
        /// <returns></returns>
        public static NewId NextId()
        {
            return NewId.Next();
        }

        /// <summary>
        /// 下一个Id
        /// <para>转字符类型</para>
        /// </summary>
        /// <returns></returns>
        public static string NextIdString()
        {
            return NextId().ToString("D");
        }

        /// <summary>
        /// 下一个Id
        /// <para>转大写</para>
        /// </summary>
        /// <returns></returns>
        public static string NextIdUpper()
        {
            return NextIdString().ToUpperInvariant();
        }

        /// <summary>
        /// 下一个Id
        /// <para>转小写</para>
        /// </summary>
        /// <returns></returns>
        public static string NextIdLower()
        {
            return NextIdString().ToLowerInvariant();
        }
    }
}
