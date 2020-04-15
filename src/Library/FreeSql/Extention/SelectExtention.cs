using FreeSql;
using Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.FreeSql.Extention
{
    public static class SelectExtention
    {

        public static Func<dynamic, TReturn> Select<TReturn>(Func<TReturn, TReturn> func) where TReturn : new()
        {
            return (a) =>
            {
                var type = typeof(TReturn);
                var result = new TReturn();
                foreach (var item in a as Dictionary<string, object>)
                {
                    type.GetProperty(item.Key, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static).SetValue(result, item.Value);
                }
                return func.Invoke(result);
            };
        }

        /// <summary>
        /// 指定返回数据
        /// </summary>
        /// <typeparam name="TReturn">返回类型</typeparam>
        /// <param name="action">处理数据</param>
        /// <returns></returns>
        public static Func<dynamic, TReturn> Select<TReturn>(Action<TReturn> action = null) where TReturn : new()
        {
            return (a) =>
            {
                var type = typeof(TReturn);
                var result = new TReturn();

                foreach (var item in a as Dictionary<string, object>)
                {
                    var prop = type.GetProperty(item.Key, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
                    object value = item.Value;
                    if (item.Value.GetType() != prop.PropertyType)
                    {
                        if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                        {
                            NullableConverter newNullableConverter = new NullableConverter(prop.PropertyType);
                            value = newNullableConverter.ConvertFrom(item.Value);
                        }
                        else
                        {
                            value = Convert.ChangeType(item.Value, prop.PropertyType);
                        }
                    }
                    prop.SetValue(result, value);
                }

                if (action != null)
                    action.Invoke(result);

                return result;
            };
        }

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
    }
}
