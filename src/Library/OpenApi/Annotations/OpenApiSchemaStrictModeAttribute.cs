using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.OpenApi.Annotations
{

    /// <summary>
    /// 接口架构严格模式
    /// <para>1：未指定OpenApiSchemaAttribute特性的属性将不会输出</para>
    /// <para>2：当存在MainTag时，未指定SubTag的属性将不会处理</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class OpenApiSchemaStrictModeAttribute : Attribute
    {

    }
}
