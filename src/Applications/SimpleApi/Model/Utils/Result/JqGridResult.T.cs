using Library.OpenApi.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Utils.Result
{
    /// <summary>
    /// JqGrid方案
    /// </summary>
    public class JqGridResult<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<T> rows { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public long total { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public long records { get; set; }

        /// <summary>
        /// 耗时（毫秒）
        /// </summary>
        public long costtime { get; set; }
    }
}
