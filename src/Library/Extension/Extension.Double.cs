using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.Extension
{
    public static partial class Extension
    {
        public static int ToInt(this double source)
        {
            return (int)Math.Round(source, 0);
        }
    }
}
