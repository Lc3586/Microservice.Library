using Library.OpenApi.Annotations;
using System.Collections.Generic;

namespace Model.Utils.Result
{
    /// <summary>
    /// AntdVue方案
    /// </summary>
    public class AntdVueResult<T>
    {
        /// <summary>
        /// 成功与否
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<T> Data { get; set; }
    }
}
