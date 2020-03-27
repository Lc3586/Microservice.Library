using FreeSql;
using Library.Models;
using Library.OpenApi.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.FreeSql.Extention
{
    public static class SelectExtention
    {
        /// <summary>
        /// 获取分页后的数据
        /// </summary>
        /// <typeparam name="TReturn">实体类型</typeparam>
        /// <param name="source"></param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        public static ISelect<TReturn> GetPagination<TReturn>(this ISelect<TReturn> source, Pagination pagination, string alias = null) where TReturn : class
        {
            string where = string.Empty;
            if (pagination.FilterToSql(ref where, alias))
            {
                if (!string.IsNullOrEmpty(where))
                    source.Where(where);
            }
            else
                throw new MessageException("搜索条件不支持");
            pagination.RecordCount = source.Count();
            string orderby = string.Empty;
            if (pagination.OrderByToSql(ref orderby, alias))
            {
                if (!string.IsNullOrEmpty(orderby))
                    source.OrderBy(orderby);
            }
            else
                throw new MessageException("排序条件不支持");
            pagination.records = source.Count();
            return source.Page(pagination.PageIndex, pagination.PageRows);
        }

        /// <summary>
        /// 返回动态类型的数据
        /// <para>根据OpenApi架构特性匹配字段<see cref="OpenApiSchemaAttribute"/></para>
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <typeparam name="TDto">业务模型类型</typeparam>
        /// <param name="orm"></param>
        /// <param name="pagination">分页菜蔬</param>
        /// <param name="otherTags">需要额外匹配的标签</param>
        /// <returns></returns>
        public static List<dynamic> ToDynamic<TSource, TDto>(this IFreeSql orm, Pagination pagination = null, params string[] otherTags) where TSource : class
        {
            var columns = orm.CodeFirst.GetTableByEntity(typeof(TSource)).ColumnsByCs;
            var fields = string.Join(
                ",",
                typeof(TDto).GetNamesWithTagAndOther(true, otherTags)
                    .Select(o => $"a.\"{columns[o].Attribute.Name}\""));

            var select = orm.Select<TSource>();
            if (pagination != null)
                select = select.AsAlias((type, old) => "a")
                                .GetPagination(pagination, "a");
            return orm.Ado.Query<dynamic>(select.ToSql(fields));
        }
    }
}
