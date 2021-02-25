using Library.OpenApi.Annotations;
using System.Collections.Generic;

namespace Model.Utils.Result
{
    /// <summary>
    /// Layui方案
    /// </summary>
    public class LayuiResult<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<T> data { get; set; }

        /// <summary>
        /// 状态码
        /// <para>成功0，失败-1</para>
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public long count { get; set; }

        /// <summary>
        /// 耗时（毫秒）
        /// </summary>
        public long costtime { get; set; }
    }
}
