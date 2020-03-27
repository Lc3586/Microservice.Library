using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.OpenApi.Annotations
{

    /// <summary>
    /// 接口架构严格模式，未指定OpenApiSchemaAttribute特性的属性将不会输出
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class OpenApiSchemaStrictModeAttribute : Attribute
    {

    }
}
