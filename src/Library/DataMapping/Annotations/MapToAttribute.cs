using System;
namespace Library.DataMapping.Annotations
{
    /// <summary>
    /// 映射目标
    /// </summary>
    public class MapToAttribute : Attribute
    {
        public MapToAttribute(Type targetType, bool enableForMember = false)
        {
            TargetType = targetType;
            EnableForMember = enableForMember;
        }
        public Type TargetType { get; }

        /// <summary>
        /// 启用单个成员自定义配置
        /// </summary>
        public bool EnableForMember { get; }
    }
}
