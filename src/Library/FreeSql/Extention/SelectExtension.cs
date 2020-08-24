using FreeSql;
using Library.Models;
using NetTaste;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.FreeSql.Extention
{
    public static class SelectExtension
    {
        /// <summary>
        /// 指定返回数据
        /// </summary>
        /// <typeparam name="TReturn">返回类型</typeparam>
        /// <param name="func">处理并返回数据</param>
        /// <returns></returns>
        public static Func<dynamic, TReturn> Select<TReturn>(Func<TReturn, TReturn> func) where TReturn : new()
        {
            return (a) =>
            {
                var result = DynamicToEntity<TReturn>(a);
                return func.Invoke(result);
            };
        }

        /// <summary>
        /// 指定返回数据
        /// </summary>
        /// <typeparam name="TReturn">返回类型</typeparam>
        /// <param name="func">处理并返回数据</param>
        /// <returns></returns>
        public static Func<dynamic, TReturn> Select<TReturn>(Func<dynamic, TReturn, TReturn> func) where TReturn : new()
        {
            return (a) =>
            {
                var result = DynamicToEntity<TReturn>(a);
                return func.Invoke(a, result);
            };
        }

        /// <summary>
        /// 指定返回数据
        /// </summary>
        /// <typeparam name="TReturn">返回类型</typeparam>
        /// <param name="action">处理数据</param>
        /// <returns></returns>
        public static Func<dynamic, TReturn> Select<TReturn>(Action<dynamic, TReturn> action = null) where TReturn : new()
        {
            return (a) =>
            {
                var result = DynamicToEntity<TReturn>(a);

                if (action != null)
                    action.Invoke(a, result);

                return result;
            };
        }

        /// <summary>
        /// 指定返回数据
        /// </summary>
        /// <typeparam name="TReturn">返回类型</typeparam>
        /// <typeparam name="T0">联表类型</typeparam>
        /// <param name="action">处理数据</param>
        /// <returns></returns>
        public static Func<dynamic, T0, TReturn> Select<TReturn, T0>(Action<dynamic, TReturn, T0> action = null) where TReturn : new()
        {
            return (a, b) =>
            {
                var result = DynamicToEntity<TReturn>(a);

                if (action != null)
                    action.Invoke(a, result, b);

                return result;
            };
        }

        /// <summary>
        /// 指定返回数据
        /// </summary>
        /// <typeparam name="TReturn">返回类型</typeparam>
        /// <typeparam name="T0">联表类型</typeparam>
        /// <typeparam name="T1">联表类型</typeparam>
        /// <param name="action">处理数据</param>
        /// <returns></returns>
        public static Func<dynamic, T0, T1, TReturn> Select<TReturn, T0, T1>(Action<dynamic, TReturn, T0, T1> action = null) where TReturn : new()
        {
            return (a, b, c) =>
            {
                var result = DynamicToEntity<TReturn>(a);

                if (action != null)
                    action.Invoke(a, result, b, c);

                return result;
            };
        }

        /// <summary>
        /// 将动态类型转为指定类型实体
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static TReturn DynamicToEntity<TReturn>(dynamic data) where TReturn : new()
        {
            var entity = new TReturn();
            var type = typeof(TReturn);
            var internal_types = new List<string>();

            foreach (var item in data as Dictionary<string, object>)
            {
                var type_value = item.Value.GetType();

                if (type_value == typeof(DBNull))
                    continue;

                var prop = type.GetProperty(item.Key, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);

                if (prop == null)
                    continue;

                object value = item.Value;

                if (type_value != prop.PropertyType)
                {
                    if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        NullableConverter newNullableConverter = new NullableConverter(prop.PropertyType);
                        try
                        {
                            if (!newNullableConverter.CanConvertFrom(item.Value.GetType()))
                            {
                                value = Convert.ChangeType(item.Value, newNullableConverter.UnderlyingType);
                            }
                            else
                                value = newNullableConverter.ConvertFrom(item.Value);
                        }
                        catch
                        {
                            value = newNullableConverter.ConvertFromString(item.Value?.ToString());
                        }
                    }
                    else
                    {
                        value = Convert.ChangeType(item.Value, prop.PropertyType);
                    }
                }
                prop.SetValue(entity, value);
            }
            return entity;
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
                if (!string.IsNullOrWhiteSpace(where))
                    source.Where(where);
            }
            else
                throw new MessageException("搜索条件不支持");
            pagination.RecordCount = source.Count();
            string orderby = string.Empty;
            if (pagination.OrderByToSql(ref orderby, alias))
            {
                if (!string.IsNullOrWhiteSpace(orderby))
                    source.OrderBy(orderby);
            }
            else
                throw new MessageException("排序条件不支持");
            pagination.records = source.Count();
            return source.Page(pagination.PageIndex, pagination.PageRows);
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
        public static List<dynamic> ToDynamic<TSource, TDto>(this ISelect<TSource> source, IFreeSql orm, Pagination pagination = null, IEnumerable<string> fields = null) where TSource : class
        {
            var columns = orm.CodeFirst.GetTableByEntity(typeof(TSource)).ColumnsByCs;

            var _fields = string.Join(
                 ",",
                 fields == null ?
                     columns.Select(o => $"a.\"{o.Value.Attribute.Name}\"") :
                     fields.Select(o => $"a.\"{columns[o].Attribute.Name}\""));

            if (pagination != null)
                source = source.AsAlias((type, old) => type == typeof(TDto) ? "a" : old)
                                .GetPagination(pagination, "a");

            return orm.Ado.Query<dynamic>(source.ToSql(_fields));
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
        public static List<dynamic> ToDynamic<TSource, TJoin0, TDto>(this ISelect<TSource> source, IFreeSql orm, Pagination pagination = null, IEnumerable<string> fields = null) where TSource : class
        {
            var table = orm.CodeFirst.GetTableByEntity(typeof(TSource));
            var table_join0 = orm.CodeFirst.GetTableByEntity(typeof(TJoin0));

            var _fields = string.Join(
                 ",",
                 fields == null ?
                     table.Columns.Concat(table_join0.Columns).Select(o => $"a.\"{o.Value.Attribute.Name}\"") :
                     fields.Select(o => table.Columns.ContainsKey(o) ?
                         $"a.\"{table.Columns[o].Attribute.Name}\"" :
                         $"a__{table_join0.DbName}.\"{table_join0.Columns[o].Attribute.Name}\""));

            if (pagination != null)
                source = source.AsAlias((type, old) => type == typeof(TDto) ? "a" : old)
                                .GetPagination(pagination, "a");

            return orm.Ado.Query<dynamic>(source.ToSql(_fields));
        }
    }
}
