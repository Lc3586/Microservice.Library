using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.OpenApi.Annotations
{
    /// <summary>
    /// 处理接口架构时忽略带有此特性的对象
    /// </summary>
    public class OpenApiIgnoreAttribute : Attribute
    {
        public OpenApiIgnoreAttribute()
        {

        }
    }
}
