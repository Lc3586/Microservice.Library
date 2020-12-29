using FreeSql;
using FreeSql.Internal.Model;
using Library.FreeSql.Application;
using Library.Models;
using NetTaste;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.FreeSql.Extention
{
    /// <summary>
    /// 查询扩展功能
    /// </summary>
    public static class SelectExtension
    {
        static SelectExtension()
        {
            foreach (var method in typeof(Enumerable).GetMethods())
            {
                switch (method.Name)
                {
                    case "Count":
                        if (method.GetParameters().Length != 1)
                            break;
                        EnumerableMethods.Add("Count", method);
                        break;
                    case "ElementAt":
                        EnumerableMethods.Add("ElementAt", method);
                        break;
                    default:
                        break;
                }
            }
        }

        #region 私有成员

        public static readonly Dictionary<string, MethodInfo> EnumerableMethods = new Dictionary<string, MethodInfo>();


        private static DynamicFilterInfo ToDynamicFilterInfo(this List<PaginationDynamicFilterInfo> paginationDynamicFilterInfo)
        {
            DynamicFilterInfo dynamicFilterInfo;
            var dynamicFilterInfos = paginationDynamicFilterInfo.Select(o => new DynamicFilterInfo
            {
                Logic = o.Relation.ToDynamicFilterLogic(),
                Field = o.Field,
                Operator = o.Compare.ToDynamicFilterOperator(),
                Value = o.Value
            });
            if (paginationDynamicFilterInfo.Count == 1)
                dynamicFilterInfo = dynamicFilterInfos.First();
            else
                dynamicFilterInfo = new DynamicFilterInfo
                {
                    Filters = dynamicFilterInfos.ToList()
                };

            return dynamicFilterInfo;
        }

        private static DynamicFilterLogic ToDynamicFilterLogic(this FilterGroupRelation filterGroupRelation)
        {
            switch (filterGroupRelation)
            {
                case FilterGroupRelation.or:
                    return DynamicFilterLogic.Or;
                case FilterGroupRelation.and:
                default:
                    return DynamicFilterLogic.And;
            }
        }

        private static DynamicFilterOperator ToDynamicFilterOperator(this FilterCompare filterCompare)
        {
            switch (filterCompare)
            {
                case FilterCompare.@in:
                    return DynamicFilterOperator.Contains;
                case FilterCompare.inStart:
                    return DynamicFilterOperator.StartsWith;
                case FilterCompare.inEnd:
                    return DynamicFilterOperator.EndsWith;
                case FilterCompare.notIn:
                    return DynamicFilterOperator.NotContains;
                case FilterCompare.notInStart:
                    return DynamicFilterOperator.NotStartsWith;
                case FilterCompare.notInEnd:
                    return DynamicFilterOperator.NotEndsWith;
                case FilterCompare.eq:
                    return DynamicFilterOperator.Eq;
                case FilterCompare.notEq:
                    return DynamicFilterOperator.NotEqual;
                case FilterCompare.le:
                    return DynamicFilterOperator.LessThanOrEqual;
                case FilterCompare.lt:
                    return DynamicFilterOperator.LessThan;
                case FilterCompare.ge:
                    return DynamicFilterOperator.GreaterThanOrEqual;
                case FilterCompare.gt:
                    return DynamicFilterOperator.GreaterThan;
                case FilterCompare.inSet:
                    return DynamicFilterOperator.Any;
                case FilterCompare.notInSet:
                    return DynamicFilterOperator.NotAny;
                case FilterCompare.range:
                    return DynamicFilterOperator.Range;
                case FilterCompare.dateRange:
                    return DynamicFilterOperator.DateRange;
                default:
                    throw new FreeSqlException($"不支持的过滤条件: {filterCompare}");
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 指定返回数据
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TReturn">返回类型</typeparam>
        /// <param name="func">处理并返回数据</param>
        /// <returns></returns>
        public static Func<TEntity, TReturn> Select<TEntity, TReturn>(Func<TEntity, TReturn> func) where TReturn : new()
        {
            return (a) =>
            {
                return func.Invoke(a);
            };
        }

        /// <summary>
        /// 指定返回数据
        /// </summary>
        /// <typeparam name="TReturn">返回类型</typeparam>
        /// <param name="func">处理并返回数据</param>
        /// <returns></returns>
        public static Func<ExpandoObject, TReturn> Select<TReturn>(Func<TReturn, TReturn> func) where TReturn : new()
        {
            return (a) =>
            {
                var result = (TReturn)DynamicToEntity(a, typeof(TReturn));
                return func.Invoke(result);
            };
        }

        /// <summary>
        /// 指定返回数据
        /// </summary>
        /// <typeparam name="TReturn">返回类型</typeparam>
        /// <param name="func">处理并返回数据</param>
        /// <returns></returns>
        public static Func<ExpandoObject, TReturn> Select<TReturn>(Func<ExpandoObject, TReturn, TReturn> func) where TReturn : new()
        {
            return (a) =>
            {
                var result = (TReturn)DynamicToEntity(a, typeof(TReturn));
                return func.Invoke(a, result);
            };
        }

        /// <summary>
        /// 指定返回数据
        /// </summary>
        /// <typeparam name="TReturn">返回类型</typeparam>
        /// <param name="action">处理数据</param>
        /// <returns></returns>
        public static Func<ExpandoObject, TReturn> Select<TReturn>(Action<ExpandoObject, TReturn> action = null) where TReturn : new()
        {
            return (a) =>
            {
                var result = (TReturn)DynamicToEntity(a, typeof(TReturn));

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
        public static Func<dynamic, T0, TReturn> Select<TReturn, T0>(Action<object, TReturn, T0> action = null) where TReturn : new()
        {
            return (a, b) =>
            {
                var result = (TReturn)DynamicToEntity(a, typeof(TReturn));

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
        public static Func<dynamic, T0, T1, TReturn> Select<TReturn, T0, T1>(Action<object, TReturn, T0, T1> action = null) where TReturn : new()
        {
            return (a, b, c) =>
            {
                var result = (TReturn)DynamicToEntity(a, typeof(TReturn));

                if (action != null)
                    action.Invoke(a, result, b, c);

                return result;
            };
        }

        /// <summary>
        /// 将动态类型转为指定类型实体
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object DynamicToEntity(ExpandoObject data, Type type)
        {
            var entity = Activator.CreateInstance(type);

            var dic = data as IDictionary<string, object>;

            foreach (var item in dic)
            {
                if (item.Value == null)
                    continue;

                var type_value = item.Value.GetType();

                if (type_value == typeof(DBNull))
                    continue;

                var prop = type.GetProperty(item.Key, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);

                if (prop == null)
                    continue;

                object value = item.Value;

                if (type_value != prop.PropertyType)
                {
                    if (type_value == typeof(List<object>))
                    {
                        var valueList = Activator.CreateInstance(prop.PropertyType);
                        var dicValueList = value as List<object>;

                        foreach (var valueItem in dicValueList)
                        {
                            prop.PropertyType.GetMethod("Add")
                                                .Invoke(valueList, new object[] {
                                                    DynamicToEntity(valueItem as ExpandoObject, prop.PropertyType.GenericTypeArguments[0])
                                                });
                        }

                        value = valueList;
                    }
                    else if (type_value == typeof(ExpandoObject))
                        value = DynamicToEntity(item.Value as ExpandoObject, prop.PropertyType);
                    else if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
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
        /// <param name="alias">表别名</param>
        /// <returns></returns>
        public static ISelect<TReturn> GetPagination<TReturn>(this ISelect<TReturn> source, Pagination pagination, string alias = null) where TReturn : class
        {
            if (pagination.DynamicFilterInfo != null && pagination.DynamicFilterInfo.Any())
                source.WhereDynamicFilter(pagination.DynamicFilterInfo.ToDynamicFilterInfo());

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

            if (pagination.PageIndex == -1)
                return source;
            else
                return source.Page(pagination.PageIndex, pagination.PageRows);
        }

        /// <summary>
        /// 获取分页后的数据
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TReturn">实体类型</typeparam>
        /// <param name="source"></param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        public static ISelectGrouping<TKey, TReturn> GetPagination<TKey, TReturn>(this ISelectGrouping<TKey, TReturn> source, Pagination pagination) where TReturn : class
        {
            //string where = string.Empty;
            //if (pagination.FilterToSql(ref where, alias))
            //{
            //    if (!string.IsNullOrWhiteSpace(where))
            //        source.Having.Where(where);
            //}
            //else
            //    throw new MessageException("搜索条件不支持");
            pagination.RecordCount = source.Count();
            //string orderby = string.Empty;
            //if (pagination.OrderByToSql(ref orderby, alias))
            //{
            //    if (!string.IsNullOrWhiteSpace(orderby))
            //        source.OrderBy(orderby);
            //}
            //else
            //    throw new MessageException("排序条件不支持");

            if (pagination.PageIndex == -1)
                return source;
            else
                return source.Page(pagination.PageIndex, pagination.PageRows);
        }

        /// <summary>
        /// 返回动态类型的数据
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <param name="source"></param>
        /// <param name="orm"></param>
        /// <param name="pagination">分页参数</param>
        /// <param name="fields">指定字段</param>
        /// <returns></returns>
        public static List<TSource> ToList<TSource>(this ISelect<TSource> source, IFreeSql orm, Pagination pagination = null, IEnumerable<string> fields = null) where TSource : class
        {
            return source.ToList<TSource, TSource>(orm, pagination, fields);
        }

        /// <summary>
        /// 返回动态类型的数据
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <typeparam name="TDto">业务模型类型</typeparam>
        /// <param name="source"></param>
        /// <param name="orm"></param>
        /// <param name="pagination">分页参数</param>
        /// <param name="fields">指定字段</param>
        /// <returns></returns>
        public static List<TDto> ToList<TSource, TDto>(this ISelect<TSource> source, IFreeSql orm, Pagination pagination = null, IEnumerable<string> fields = null) where TSource : class
        {
            var table = orm.GetTableInfo<TSource>();
            var columns = table.ColumnsByCs.Select(c => $"{(fields?.Contains(c.Key) != true ? "NULL as " : string.Empty)}\"{c.Key}\"");

            if (pagination != null)
                source = source.AsAlias((type, old) => type == typeof(TDto) ? "a" : old)
                                .GetPagination(pagination, "a");

            return source.WithSql($"SELECT {string.Join(",", columns)} FROM \"{table.DbName}\"").ToList<TDto>();
        }

        /// <summary>
        /// 返回动态类型的数据
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <typeparam name="TDto">业务模型类型</typeparam>
        /// <param name="source"></param>
        /// <param name="orm"></param>
        /// <param name="pagination">分页参数</param>
        /// <param name="fields">指定字段</param>
        /// <returns></returns>
        [Obsolete("此方法不支持参数化,请使用ToList<TSource, TDto>")]
        public static List<dynamic> ToDynamic<TSource, TDto>(this ISelect<TSource> source, IFreeSql orm, Pagination pagination = null, IEnumerable<string> fields = null) where TSource : class
        {
            var columns = orm.GetTableInfo<TSource>().ColumnsByCs;

            var _fields = string.Join(
                 ",",
                 fields == null ?
                     columns.Select(o => $"a.\"{o.Value.Attribute.Name}\"") :
                     fields.Select(o => $"a.\"{(columns.ContainsKey(o) ? columns[o].Attribute.Name : o)}\""));

            if (pagination != null)
                source = source.AsAlias((type, old) => type == typeof(TDto) ? "a" : old)
                                .GetPagination(pagination, "a");

            return orm.Ado.Query<dynamic>(source.ToSql(_fields));
        }

        /// <summary>
        /// 返回动态类型的数据
        /// </summary>
        /// <typeparam name="TSource">实体类型</typeparam>
        /// <typeparam name="TJoin0"></typeparam>
        /// <typeparam name="TDto">业务模型类型</typeparam>
        /// <param name="source"></param>
        /// <param name="orm"></param>
        /// <param name="pagination">分页参数</param>
        /// <param name="fields">指定字段</param>
        /// <returns></returns>
        [Obsolete("此方法不支持参数化,请使用ToList<TSource, TDto>")]
        public static List<dynamic> ToDynamic<TSource, TJoin0, TDto>(this ISelect<TSource> source, IFreeSql orm, Pagination pagination = null, IEnumerable<string> fields = null) where TSource : class
        {
            var table = orm.GetTableInfo<TSource>();
            var table_join0 = orm.GetTableInfo<TJoin0>();

            var _fields = string.Join(
                 ",",
                 fields == null ?
                     table.Columns.Concat(table_join0.Columns).Select(o => $"a.\"{o.Value.Attribute.Name}\"") :
                     fields.Select(o => table.Columns.ContainsKey(o) || table_join0.Columns.ContainsKey(o) ? (table.Columns.ContainsKey(o) ?
                         $"a.\"{table.Columns[o].Attribute.Name}\"" :
                         $"a__{table_join0.DbName}.\"{table_join0.Columns[o].Attribute.Name}\"") : $"a.\"{o}\""));

            if (pagination != null)
                source = source.AsAlias((type, old) => type == typeof(TDto) ? "a" : old)
                                .GetPagination(pagination, "a");

            return orm.Ado.Query<dynamic>(source.ToSql(_fields));
        }

        #endregion
    }
}
