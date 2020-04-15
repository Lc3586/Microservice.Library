using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data.Common;

namespace Library.DataMapping.Application
{
    public class AutoMapperGeneratorOptions
    {
        public AutoMapperGeneratorOptions()
        {

        }

        /// <summary>
        /// 类型集合
        /// </summary>
        public List<Type> Types { get; set; }
    }
}
