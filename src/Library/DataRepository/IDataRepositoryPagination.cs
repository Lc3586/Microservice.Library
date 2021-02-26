using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microservice.Library.DataRepository
{
    /// <summary>
    /// 分页设置
    /// </summary>
    public interface IDataRepositoryPagination
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 每页数据量
        /// </summary>
        int PageRows { get; set; }

        /// <summary>
        /// 排序列
        /// </summary>
        string SortField { get; set; }

        /// <summary>
        /// 排序类型
        /// </summary>
        /// <remarks>
        /// <para>asc</para>
        /// <para>desc</para>
        /// </remarks>
        string SortType { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        long RecordCount { get; set; }

        /// <summary>
        /// 过滤
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable<T> DataRepositoryFilter<T>(IQueryable<T> query);

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable<T> DataRepositoryOrderBy<T>(IQueryable<T> query);
    }
}
