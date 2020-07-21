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
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="enableMemberMap">启用成员映射</param>
        public MapFromAttribute(Type type, bool enableMemberMap = false)
        {
            Type = type;
            EnableMemberMap = enableMemberMap;
        }

        public bool IsFrom => true;

        /// <summary>
        /// 类型
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// 启用单个成员自定义配置
        /// </summary>
        public bool EnableMemberMap { get; }
    }
}
