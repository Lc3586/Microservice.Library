using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.OpenApi.Annotations
{
    public static class OpenApiSchemaDefaultValue
    {
        public static bool _bool = true;
        public static byte _byte = 1;
        public static byte[] _byte_array = new byte[] { };
        public static int _int = 1;
        public static long _long = 1L;
        public static float _float = 1F;
        public static double _double = 1D;
        public static string _string = string.Empty;
        public static DateTime _dateTime = DateTime.UtcNow;
    }
}
