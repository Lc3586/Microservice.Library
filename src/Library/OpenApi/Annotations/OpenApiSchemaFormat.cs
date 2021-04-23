using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.OpenApi.Annotations
{
    /// <summary>
    /// 接口架构格式
    /// </summary>
    public class OpenApiSchemaFormat
    {
        /// <summary>
        /// 字节
        /// </summary>
        public const string integer_byte = "byte";

        /// <summary>
        /// 字节数组
        /// </summary>
        public const string integer_byte_array = "byte[]";

        /// <summary>
        /// 二进制数组
        /// </summary>
        public const string integer_binary = "binary";

        /// <summary>
        /// 32位整数
        /// </summary>
        public const string integer_int32 = "int32";

        /// <summary>
        /// 64位整数
        /// </summary>
        public const string integer_int64 = "int64";

        /// <summary>
        /// 单精度浮点数
        /// </summary>
        public const string number_float = "float";

        /// <summary>
        /// 双精度浮点数
        /// </summary>
        public const string number_double = "double";

        /// <summary>
        /// 高精度浮点数
        /// </summary>
        public const string number_decimal = "decimal";

        /// <summary>
        /// 日期
        /// </summary>
        public const string string_date = "date";

        /// <summary>
        /// 日期和时间
        /// </summary>
        public const string string_datetime = "date-time";

        /// <summary>
        /// 时间
        /// </summary>
        public const string string_time = "time";

        /// <summary>
        /// 时间跨度
        /// </summary>
        public const string string_timespan = "timespan";

        /// <summary>
        /// 原始时间
        /// </summary>
        public const string string_date_original = "date-original";

        /// <summary>
        /// 密文
        /// </summary>
        public const string string_password = "password";

        /// <summary>
        /// 包含注释
        /// </summary>
        public const string enum_description = "description";

        /// <summary>
        /// 只解析一次
        /// <para>防止无限递归</para>
        /// </summary>
        public const string model_once = "once";
    }
}
