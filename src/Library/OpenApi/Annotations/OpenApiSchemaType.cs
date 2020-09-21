using System;
using System.Collections.Generic;
using System.Text;

namespace Library.OpenApi.Annotations
{
    /// <summary>
    /// 接口架构类型
    /// </summary>
    public class OpenApiSchemaType
    {
        /// <summary>
        /// 布尔
        /// </summary>
        public const string @boolean = "boolean";

        /// <summary>
        /// 整数
        /// </summary>
        public const string integer = "integer";

        /// <summary>
        /// 数值
        /// </summary>
        public const string number = "number";

        /// <summary>
        /// 字符串
        /// </summary>
        public const string @string = "string";

        /// <summary>
        /// 枚举
        /// </summary>
        public const string @enum = "enum";

        /// <summary>
        /// 嵌套模型
        /// </summary>
        public const string model = "model";
    }
}
