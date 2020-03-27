using System;
using System.Collections.Generic;
using System.Text;

namespace Library.OpenApi.Annotations
{
    /// <summary>
    /// 接口架构属性
    /// </summary>
    /// <remarks>LCTR 2020-03-10</remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class OpenApiSchemaAttribute : Attribute
    {
        /// <summary>
        /// 
        /// <para><paramref name="type" cref="OpenApiSchemaType"/></para>
        /// <para><paramref name="format" cref="OpenApiSchemaFormat"/></para>
        /// </summary>
        /// <param name="type" >类型</param>
        /// <param name="format">格式</param>
        /// <param name="value">示例值</param>
        public OpenApiSchemaAttribute(string type, string format = null, object value = null)
        {
            Type = type;
            Format = format;
            Value = value;
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 格式
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 示例值
        /// </summary>
        public object Value { get; set; }
    }
}
