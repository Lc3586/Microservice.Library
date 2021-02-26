using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.Extension
{
    /// <summary>
    /// 拓展类
    /// </summary>
    public static partial class Extension
    {
        /// <summary>
        /// JavaScript时间戳转换为C# DateTime
        /// </summary>
        /// <param name="jsTimestamp">JavaScript时间戳</param>
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
