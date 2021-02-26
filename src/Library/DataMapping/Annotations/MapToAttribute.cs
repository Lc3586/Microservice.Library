using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservice.Library.DataMapping.Annotations
{
    /// <summary>
    /// 映射目标配置
    /// </summary>
    /// <remarks>
    /// <para>可使用 <see cref="Application.IMapTo{TSource, TDestination}"/>接口类添加成员映射配置</para>
    /// </remarks>
    public class MapToAttribute : Attribute, IMapAttribute
    {
        /// <summary>
        /// 映射目标
        /// </summary>
        /// <param name="types">目标类型集合</param>
        public MapToAttribute(params Type[] types)
        {
            Types = types?.ToList();
        }

        public bool FromOrTo => false;

        /// <summary>
        /// 目标类型集合
        /// </summary>
        public List<Type> Types { get; }
    }
}
