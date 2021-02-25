using Library.OpenApi.Annotations;
using System.Collections.Generic;

namespace Model.Utils.Result
{

    /// <summary>
    /// ElementVue方案数据信息
    /// </summary>
    public class ElementVueResultData<T>
    {
        /// <summary>
        /// 总页数
        /// </summary>
        public long PageTotal { get; internal set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public long Total { get; internal set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; internal set; }

        /// <summary>
        /// 每页数据量
        /// </summary>
        public int PageSize { get; internal set; }

        /// <summary>
        /// 数据
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<T> List { get; internal set; }
    }
}
