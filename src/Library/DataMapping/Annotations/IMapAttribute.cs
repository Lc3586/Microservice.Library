using System;
using System.Collections.Generic;

namespace Microservice.Library.DataMapping.Annotations
{
    /// <summary>
    /// 数据映射配置
    /// </summary>
    internal interface IMapAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// <para>true <see cref="MapFromAttribute"/></para>
        /// <para>false <see cref="MapToAttribute"/></para>
        /// </remarks>
        bool FromOrTo { get; }

        /// <summary>
        /// 类型集合
        /// </summary>
        List<Type> Types { get; }
    }
}
