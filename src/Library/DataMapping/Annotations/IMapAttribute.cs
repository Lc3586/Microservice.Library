using System;
using System.Collections.Generic;
using System.Text;

namespace Library.DataMapping.Annotations
{
    public interface IMapAttribute
    {
        /// <summary>
        /// 是否来源
        /// </summary>
        bool IsFrom { get; }

        /// <summary>
        /// 类型
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// 启用单个成员自定义配置
        /// </summary>
        bool EnableMemberMap { get; }
    }
}
