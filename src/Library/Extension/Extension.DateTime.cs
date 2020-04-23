using NodaTime;
using System;
using System.Globalization;

namespace Library.Extension
{
    public static partial class Extension
    {
        ///   <summary> 
        ///  获取某一日期是该年中的第几周
        ///   </summary> 
        ///   <param name="dateTime"> 日期 </param> 
        ///   <returns> 该日期在该年中的周数 </returns> 
        public static int GetWeekOfYear(this DateTime dateTime)
        {
            GregorianCalendar gc = new GregorianCalendar();
            return gc.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        /// <summary>
        /// 获取Js格式的timestamp
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <returns></returns>
        public static long ToJsTimestamp(this DateTime dateTime)
        {
            return (dateTime.Ticks - new DateTime().LocalDefault().Ticks) / 10000;   //除10000调整为13位
        }

        /// <summary>
        /// 获取js中的getTime()
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns></returns>
        public static long JsGetTime(this DateTime dt)
        {
            long retval = 0;
            var st = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan t = (dt.ToUniversalTime() - st);
            retval = (Int64)(t.TotalMilliseconds + 0.5);
            return retval;
        }

        /// <summary>
        /// 获取Unix时间戳
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns></returns>
        public static long ToUnixTimestamp(this DateTime dt)
        {
            return (long)(dt - new DateTime().LocalDefault()).TotalSeconds;
        }

        /// <summary>
        /// 返回默认时间1970-01-01
        /// </summary>
        /// <param name="dt">时间日期</param>
        /// <returns></returns>
        public static DateTime Default(this DateTime dt)
        {
            return new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
        }

        /// <summary>
        /// 返回默认时间1970-01-01
        /// </summary>
        /// <param name="dt">时间日期</param>
        /// <returns></returns>
        public static DateTime LocalDefault(this DateTime dt)
        {
            return TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0), TimeZoneInfo.Local);
        }

        /// <summary>
        /// 转为标准时间（北京时间，解决Linux时区问题）
        /// </summary>
        /// <param name="dt">当前时间</param>
        /// <returns></returns>
        public static DateTime ToCstTime(this DateTime dt)
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            var shanghaiZone = DateTimeZoneProviders.Tzdb["Asia/Shanghai"];
            return now.InZone(shanghaiZone).ToDateTimeUnspecified();
        }
    }
}
