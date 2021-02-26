using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data.Common;

namespace Microservice.Library.DataMapping.Application
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

        /// <summary>
        /// 启用映射来源
        /// </summary>
        public bool EnableMapFrom { get; set; } = true;

        /// <summary>
        /// 启用映射目标
        /// </summary>
        public bool EnableMapTo { get; set; } = true;
    }
}
