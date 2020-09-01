using System;

namespace Library.DataMapping.Annotations
{
    /// <summary>
    /// 映射来源
    /// </summary>
    public class MapFromAttribute : Attribute, IMapAttribute
    {
        /// <summary>
        /// 映射来源
        /// <para>如要设置成员映射选项, 请在类中设置公共的静态字段或属性<![CDATA[MemberMapOptions<TSource, TDestination> FromMemberMapOptions]]>, 并调用Add方法赋值</para>
        /// </summary>
        /// <param name="type">类型</param>
        public MapFromAttribute(params Type[] type)
        {
            Type = type;
        }

        public bool IsFrom => true;

        /// <summary>
        /// 类型
        /// </summary>
        public Type[] Type { get; }
    }
}
