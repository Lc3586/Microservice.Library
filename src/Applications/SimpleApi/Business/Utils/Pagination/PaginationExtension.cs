using Model.Utils.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Business.Utils.Pagination
{
    /// <summary>
    /// 分页设置拓展方法
    /// </summary>
    public static class PaginationExtension
    {
        /// <summary>
        /// 筛选条件转sql语句
        /// </summary>
        /// <param name="pagination">分页设置</param>
        /// <param name="sql">sql语句</param>
        /// <param name="alias">别名</param>
        /// <returns>筛选条件是否有误</returns>
        public static bool FilterToSql(this PaginationDTO pagination, ref string sql, string alias = null)
        {
            if (pagination.Filter == null || !pagination.Filter.Any())
                return true;
            try
            {
                string predicate = string.Empty;

                for (int i = 0, j = i; i < pagination.Filter.Count; i++)
                {
                    var filter = pagination.Filter[i];
                    if (filter == null)
                        continue;

                    if (filter.Group?.Flag == FilterGroupFlag.start)
                        predicate += "(";

                    string field = filter.Field;
                    if (alias != null)
                        field = $"{alias}.\"{field}\"";
                    else
                        field = $"\"{field}\"";

                    string value = filter.Value?.ToString();
                    if (filter.ValueIsField)
                    {

                        if (alias != null)
                            value = $"{alias}.\"{value}\"";
                        else
                            value = $"\"{value}\"";
                    }

                    bool skip = false;
                    switch (filter.Compare)
                    {
                        case FilterCompare.@in:
                            if (string.IsNullOrEmpty(value))
                            {
                                skip = true;
                                break;
                            }
                            if (filter.ValueIsField)
                                predicate += $"{field} LIKE {value}";
                            else
                                predicate += $"{field} LIKE '{value}'";
                            break;
                        case FilterCompare.includedIn:
                            if (string.IsNullOrEmpty(value))
                            {
                                skip = true;
                                break;
                            }
                            if (filter.ValueIsField)
                                predicate += $"{value} LIKE CONCAT('%',{field},'%')";
                            else
                                predicate += $"'{value}' LIKE CONCAT('%',{field},'%')";
                            break;
                        case FilterCompare.eq:
                            if (value == null)
                                predicate += $"{field} is null";
                            else
                            {
                                if (filter.ValueIsField)
                                    predicate += $"{field} = {value}";
                                else
                                    predicate += $"{field} = '{value}'";
                            }
                            break;
                        case FilterCompare.notEq:
                            if (value == null)
                                predicate += $"{field} is not null";
                            else
                            {
                                if (filter.ValueIsField)
                                    predicate += $"{field} != {value}";
                                else
                                    predicate += $"{field} != '{value}'";
                            }
                            break;
                        case FilterCompare.le:
                            if (filter.ValueIsField)
                                predicate += $"{field} <= {value}";
                            else
                                predicate += $"{field} <= '{value}'";
                            break;
                        case FilterCompare.lt:
                            if (filter.ValueIsField)
                                predicate += $"{field} < {value}";
                            else
                                predicate += $"{field} < '{value}'";
                            break;
                        case FilterCompare.ge:
                            if (filter.ValueIsField)
                                predicate += $"{field} >= {value}";
                            else
                                predicate += $"{field} >= '{value}'";
                            break;
                        case FilterCompare.gt:
                            if (filter.ValueIsField)
                                predicate += $"{field} > {value}";
                            else
                                predicate += $"{field} > '{value}'";
                            break;
                        case FilterCompare.inSet:
                            predicate += $"{field} IN ({value})";
                            break;
                        case FilterCompare.notInSet:
                            predicate += $"{field} NOT IN ({value})";
                            break;
                        default:
                            skip = true;
                            break;
                    }

                    if (filter.Group?.Flag == FilterGroupFlag.end)
                        predicate += ")";

                    if (!skip && i != pagination.Filter.Count - 1)
                    {
                        string relation = filter.Group?.Relation.ToString();
                        switch (relation)
                        {
                            case "or":
                                predicate += $" OR ";
                                break;
                            case "and":
                            default:
                                predicate += $" AND ";
                                break;
                        }
                    }

                    if (!skip)
                        j++;
                }

                if (!string.IsNullOrWhiteSpace(predicate))
                    sql += ' ' + predicate + ' ';

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 排序条件转sql语句
        /// </summary>
        /// <param name="pagination">分页设置</param>
        /// <param name="sql">sql语句</param>
        /// <param name="alias">别名</param>
        /// <returns>排序条件是否有误</returns>
        public static bool OrderByToSql(this PaginationDTO pagination, ref string sql, string alias = null)
        {
            try
            {
                string predicate = string.Empty;

                if (pagination.AdvancedSort != null && pagination.AdvancedSort.Any())
                {
                    foreach (var item in pagination.AdvancedSort)
                    {
                        if (item == null)
                            continue;

                        string field = item.Field;
                        if (alias != null)
                            field = $" {alias}.\"{field}\" ";
                        else
                            field = $" \"{field}\" ";
                        string type = item.Type.ToString();


                        if (string.IsNullOrEmpty(type))
                            type = "asc";

                        if (!string.IsNullOrEmpty(predicate))
                            predicate += ',';

                        predicate += $" {field} {type} ";
                    }
                }
                else if (!string.IsNullOrEmpty(pagination.SortField))
                {
                    if (alias != null)
                        pagination.SortField = $" {alias}.\"{pagination.SortField}\" ";
                    else
                        pagination.SortField = $" \"{pagination.SortField}\" ";

                    predicate += $" {pagination.SortField} {pagination.SortType} ";
                }

                if (!string.IsNullOrWhiteSpace(predicate))
                    sql += ' ' + predicate + ' ';

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 筛选条件转动态linq
        /// </summary>
        /// <param name="pagination">分页设置</param>
        /// <param name="LinqDynamic">动态linq</param>
        /// <returns>筛选条件是否有误</returns>
        public static bool FilterToLinqDynamic<TSource>(this PaginationDTO pagination, ref IQueryable<TSource> LinqDynamic)
        {
            try
            {
                if (pagination.Filter == null || !pagination.Filter.Any())
                    return true;

                string predicate = string.Empty;
                List<object> args = new List<object>();

                for (int i = 0, j = i; i < pagination.Filter.Count; i++)
                {
                    var filter = pagination.Filter[i];
                    if (filter == null)
                        continue;

                    if (filter.Group?.Flag == FilterGroupFlag.start)
                        predicate += "(";

                    string field = filter.Field;
                    object value = filter.Value;

                    bool skip = false;
                    switch (filter.Compare)
                    {
                        case FilterCompare.@in:
                            if (string.IsNullOrEmpty(value.ToString()))
                            {
                                skip = true;
                                break;
                            }
                            predicate += $"{field}.Contains({(filter.ValueIsField ? "" : "@")}{j})";
                            args.Add(value);
                            break;
                        case FilterCompare.includedIn:
                            if (string.IsNullOrEmpty(value.ToString()))
                            {
                                skip = true;
                                break;
                            }
                            predicate += $"{(filter.ValueIsField ? "" : "@")}{j}.Contains({field})";
                            args.Add(value);
                            break;
                        case FilterCompare.eq:
                            predicate += $"{field} == {(filter.ValueIsField ? "" : "@")}{j}";
                            args.Add(value);
                            break;
                        case FilterCompare.notEq:
                            predicate += $"!{field}.Equals({(filter.ValueIsField ? "" : "@")}{j})";
                            args.Add(value);
                            break;
                        case FilterCompare.le:
                            predicate += $"{field} <= {(filter.ValueIsField ? "" : "@")}{j}";
                            args.Add(value);
                            break;
                        case FilterCompare.lt:
                            predicate += $"{field} < {(filter.ValueIsField ? "" : "@")}{j}";
                            args.Add(value);
                            break;
                        case FilterCompare.ge:
                            predicate += $"{field} >= {(filter.ValueIsField ? "" : "@")}{j}";
                            args.Add(value);
                            break;
                        case FilterCompare.gt:
                            predicate += $"{field} > {(filter.ValueIsField ? "" : "@")}{j}";
                            args.Add(value);
                            break;
                        case FilterCompare.inSet:
                        case FilterCompare.notInSet:
                        default:
                            skip = true;
                            break;
                    }

                    if (filter.Group?.Flag == FilterGroupFlag.end)
                        predicate += ")";

                    if (!skip && i != pagination.Filter.Count - 1)
                    {
                        string relation = filter.Group.Relation.ToString();
                        switch (filter.Group.Relation)
                        {
                            case FilterGroupRelation.and:
                                predicate += $" && ";
                                break;
                            case FilterGroupRelation.or:
                                predicate += $" || ";
                                break;
                            default:
                                break;
                        }
                    }

                    if (!skip)
                        j++;
                }

                if (!string.IsNullOrWhiteSpace(predicate))
                    LinqDynamic = (IQueryable<TSource>)typeof(DynamicQueryableExtensions)
                                                .GetMethods()
                                                .FirstOrDefault(o =>
                                                                    o.Name == "Where"
                                                                    && o.IsGenericMethod
                                                                    && o.GetParameters().Count(p =>
                                                                                                    !new[] { typeof(IQueryable<>), typeof(string), typeof(object[]) }
                                                                                                    .Contains(p.ParameterType.IsGenericType ? p.ParameterType.GetGenericTypeDefinition() : p.ParameterType)
                                                                                              ) == 0
                                                                )
                                               .MakeGenericMethod(typeof(TSource))
                                               .Invoke(null, new object[] { LinqDynamic, predicate, args.ToArray() });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 排序条件转动态linq
        /// </summary>
        /// <param name="pagination">分页设置</param>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="LinqDynamic">动态linq</param>
        /// <returns>排序条件是否有误</returns>
        public static bool OrderByToLinqDynamic<TSource>(this PaginationDTO pagination, ref IQueryable<TSource> LinqDynamic)
        {
            try
            {
                if (pagination.AdvancedSort != null && pagination.AdvancedSort.Any())
                {
                    IOrderedQueryable<TSource> orderByLinq = null;

                    foreach (var item in pagination.AdvancedSort)
                    {
                        if (item == null)
                            continue;

                        string field = item.Field;
                        string type = item.Type.ToString();

                        if (orderByLinq == null)
                            orderByLinq = LinqDynamic.OrderBy($"{field} {type}");
                        else
                            orderByLinq = orderByLinq.ThenBy($"{field} {type}");
                    }

                    LinqDynamic = orderByLinq;
                }
                else if (!string.IsNullOrEmpty(pagination.SortField))
                {
                    LinqDynamic = LinqDynamic.OrderBy($"{pagination.SortField} {pagination.SortType}");
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
