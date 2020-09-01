using System;
namespace Library.DataMapping.Annotations
{
    /// <summary>
    /// 映射目标
    /// </summary>
    public class MapToAttribute : Attribute, IMapAttribute
    {
        /// <summary>
        /// 映射目标
        /// </summary>
        /// <param name="type">类型</param>
        public MapToAttribute(params Type[] type)
        {
            Type = type;
        }

        public bool IsFrom => false;

        /// <summary>
        /// 类型
        /// </summary>
        public Type[] Type { get; }
    }
}
