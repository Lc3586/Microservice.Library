using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Json.Converters
{
    public class DateTimeConverter : IsoDateTimeConverter
    {
        public DateTimeConverter(string format) : base()
        {
            base.DateTimeFormat = format;
        }
    }
}
