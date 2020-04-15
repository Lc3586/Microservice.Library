using System;

namespace Library.DataMapping.Annotations
{
    /// <summary>
    /// 映射来源
    /// </summary>
    public class MapFromAttribute : Attribute
    {
        public MapFromAttribute(Type fromType, bool enableForMember = false)
        {
            FromType = fromType;
            EnableForMember = enableForMember;
        }
        public Type FromType { get; }

        /// <summary>
        /// 启用单个成员自定义配置
        /// </summary>
        public bool EnableForMember { get; }
    }
}
