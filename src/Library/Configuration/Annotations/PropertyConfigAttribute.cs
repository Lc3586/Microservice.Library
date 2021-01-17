using System;

namespace Library.Configuration.Annotations
{
    /// <summary>
    /// 属性配置
    /// </summary>
    /// <remarks>检查类型下所有设置了<see cref="JsonConfigAttribute"/>特性的属性</remarks>
    [AttributeUsage(AttributeTargets.Property)]

    public class PropertyConfigAttribute : Attribute
    {

    }
}
