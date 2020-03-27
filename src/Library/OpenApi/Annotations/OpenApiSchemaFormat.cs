using System;
using System.Collections.Generic;
using System.Text;

namespace Library.OpenApi.Annotations
{
    /// <summary>
    /// 接口架构格式
    /// </summary>
    public class OpenApiSchemaFormat
    {
        public const string integer_byte = "byte";
        public const string integer_byte_array = "byte[]";
        public const string integer_binary = "binary";
        public const string integer_int32 = "int32";
        public const string integer_int64 = "int64";
        public const string number_float = "float";
        public const string number_double = "double";
        public const string number_decimal = "decimal";
        public const string string_date = "date";
        public const string string_datetime = "date-time";
        public const string string_date_original = "date-original";
        public const string string_password = "password";
    }
}
