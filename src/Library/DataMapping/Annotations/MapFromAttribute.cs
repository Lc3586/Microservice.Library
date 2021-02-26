using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservice.Library.DataMapping.Annotations
{
    /// <summary>
    /// 映射来源配置
    /// </summary>
    /// <remarks>
    /// <para>可使用 <see cref="Application.IMapFrom{TSource, TDestination}"/>接口类添加成员映射配置</para>
    /// </remarks>
    public class MapFromAttribute : Attribute, IMapAttribute
    {
        /// <summary>
        /// 映射来源
        /// </summary>
        /// <param name="types">来源类型集合</param>
        public MapFromAttribute(params Type[] types)
        {
            Types = types?.ToList(); ;
        }

        public bool FromOrTo => true;

        /// <summary>
        /// 来源类型集合
        /// </summary>
        public List<Type> Types { get; }
    }
}
