using NodaTime;
using System;

namespace Microservice.Library.Cache.Extention
{
    /// <summary>
    /// 日期拓展方法
    /// </summary>
    public static class DateTimeExtention
    {
        /// <summary>
        /// 获取标准时间（北京时间，解决Linux时区问题）
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCstTime()
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            var shanghaiZone = DateTimeZoneProviders.Tzdb["Asia/Shanghai"];
            return now.InZone(shanghaiZone).ToDateTimeUnspecified();
        }
    }
}
