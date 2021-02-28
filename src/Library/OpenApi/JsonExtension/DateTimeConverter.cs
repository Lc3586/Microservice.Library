using Newtonsoft.Json.Converters;

namespace Microservice.Library.OpenApi.JsonExtension
{
    /// <summary>
    /// 日期时间转换器
    /// </summary>
    public class DateTimeConverter : IsoDateTimeConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format">格式化字符串</param>
        public DateTimeConverter(string format) : base()
        {
            base.DateTimeFormat = format;
        }
    }
}
