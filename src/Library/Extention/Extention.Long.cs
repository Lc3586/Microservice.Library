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
        /// JavaScript时间戳转换为C# DateTime
        /// </summary>
        /// <param name="unixTimestamp">JavaScript时间戳</param>
        public static DateTime JSTimestampToDatetime(this long jsTimestamp)
        {
            return new DateTime().LocalDefault().AddMilliseconds(jsTimestamp);
        }

        /// <summary>
        /// Unix时间戳转换为C# DateTime
        /// </summary>
        /// <param name="unixTimestamp">Unix时间戳</param>
        public static DateTime UnixTimestampToDatetime(this long unixTimestamp)
        {
            return new DateTime().LocalDefault().AddSeconds(unixTimestamp);
        }
    }
}
