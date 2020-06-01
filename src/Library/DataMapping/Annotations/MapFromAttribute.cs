using System;

namespace Library.DataMapping.Annotations
{
    /// <summary>
    /// 映射来源
    /// </summary>
    public class MapFromAttribute : Attribute
    {
        /// <summary>
        /// 映射来源
        /// </summary>
        /// <param name="fromType">类型</param>
        /// <param name="enableForMember">启用单个成员自定义配置</param>
        public MapFromAttribute(Type fromType, bool enableForMember = false)
        {
            FromType = fromType;
            EnableForMember = enableForMember;
        }

        /// <summary>
        /// 类型
        /// </summary>
        public Type FromType { get; }

        /// <summary>
        /// 启用单个成员自定义配置
        /// </summary>
        public bool EnableForMember { get; }
    }
}
