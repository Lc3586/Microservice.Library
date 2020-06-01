using System;
namespace Library.DataMapping.Annotations
{
    /// <summary>
    /// 映射目标
    /// </summary>
    public class MapToAttribute : Attribute
    {
        /// <summary>
        /// 映射目标
        /// </summary>
        /// <param name="targetType">类型</param>
        /// <param name="enableForMember">启用单个成员自定义配置</param>
        public MapToAttribute(Type targetType, bool enableForMember = false)
        {
            TargetType = targetType;
            EnableForMember = enableForMember;
        }

        /// <summary>
        /// 类型
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        /// 启用单个成员自定义配置
        /// </summary>
        public bool EnableForMember { get; }
    }
}
