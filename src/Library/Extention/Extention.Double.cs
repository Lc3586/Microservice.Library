using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Extention
{
    public static partial class Extention
    {
        public static int ToInt(this double source)
        {
            return (int)Math.Round(source, 0);
        }
    }
}
