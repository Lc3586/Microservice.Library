using Library.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Library.FreeSql.Extention
{
    public static class OrmExtention
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orm"></param>
        /// <param name="handler">事务体</param>
        /// <param name="timeout">超时设置</param>
        /// <param name="isolationLevel">事务隔离级别</param>
        /// <returns></returns>
        public static (bool Success, Exception Ex) RunTransaction(this IFreeSql orm, Action handler, TimeSpan? timeout = null, IsolationLevel? isolationLevel = null)
        {
            try
            {
                if (isolationLevel != null)
                    orm.Transaction(isolationLevel.Value, timeout.Value, handler);
                else if (timeout != null)
                    orm.Transaction(timeout.Value, handler);
                else
                    orm.Transaction(handler);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex);
            }
        }

        /// <summary>
        /// 返回动态类型的数据
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <typeparam name="TDto">业务模型类型</typeparam>
        /// <param name="orm"></param>
        /// <param name="pagination">分页菜蔬</param>
        /// <param name="fields">指定字段</param>
        /// <returns></returns>
        public static List<dynamic> ToDynamic<TSource, TDto>(this IFreeSql orm, Pagination pagination = null, IEnumerable<string> fields = null) where TSource : class
        {
            var columns = orm.CodeFirst.GetTableByEntity(typeof(TSource)).ColumnsByCs;

            var _fields = string.Join(
                 ",",
                 fields == null ?
                     columns.Select(o => $"a.\"{o.Value.Attribute.Name}\"") :
                     fields.Select(o => $"a.\"{columns[o].Attribute.Name}\""));

            var select = orm.Select<TSource>();
            if (pagination != null)
                select = select.AsAlias((type, old) => "a")
                                .GetPagination(pagination, "a");

            return orm.Ado.Query<dynamic>(select.ToSql(_fields));
        }
    }
}
