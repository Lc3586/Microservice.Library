using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.OpenApi.Annotations
{
    /// <summary>
    /// 主标签
    /// <para>主要的，用于类</para>
    /// <para>和附属标签搭配使用</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class OpenApiMainTagAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">名称</param>
        public OpenApiMainTagAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; }
    }

    /// <summary>
    /// 附属标签
    /// <para>标明所属，用于属性和字段</para>
    /// <para>和主标签搭配使用</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class OpenApiSubTagAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">名称</param>
        public OpenApiSubTagAttribute(params string[] name)
        {
            Name = name.ToList();
        }

        /// <summary>
        /// 名称
        /// </summary>
        public List<string> Name { get; }
    }
}
