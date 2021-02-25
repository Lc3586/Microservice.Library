using Library.OpenApi.Annotations;
using System.Collections.Generic;

namespace Model.Utils.Result
{
    /// <summary>
    /// Easyui DataGrid方案
    /// </summary>
    public class EasyuiResult<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<T> rows { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public long total { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int currentPage { get; set; }

        /// <summary>
        /// 每页数据量
        /// </summary>
        public int pageSize { get; set; }

        /// <summary>
        /// 耗时（毫秒）
        /// </summary>
        public long costtime { get; set; }
    }
}
