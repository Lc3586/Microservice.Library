using Library.OpenApi.Annotations;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Library.Models
{
    /// <summary>
    /// 数据表格分页
    /// </summary>
    [OpenApiSchemaStrictMode]
    [OpenApiMainTag("Pagination")]
    public class Pagination
    {
        #region 构造函数

        public Pagination()
        {
            _watch.Start();

            _sortField = "Id";
            _sortType = "asc";
            PageIndex = 1;
            PageRows = 50;
            _schema = Schema.defaul;
        }

        #endregion

        #region 私有成员

        /// <summary>
        /// 架构
        /// </summary>
        private Schema _schema { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        private int _pageIndex { get; set; }

        /// <summary>
        /// 每页行数
        /// </summary>
        private int _pageRows { get; set; }

        /// <summary>
        /// 排序列
        /// </summary>
        private string _sortField { get; set; }

        /// <summary>
        /// 排序类型
        /// </summary>
        private string _sortType { get; set; }

        /// <summary>
        /// 高级排序
        /// </summary>
        private string _advancedSort { get; set; }

        /// <summary>
        /// 筛选条件
        /// </summary>
        private string _filter { get; set; }

        /// <summary>
        /// 筛选条件转sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="alias">别名</param>
        /// <returns>筛选条件是否有误</returns>
        public bool FilterToSql(ref string sql, string alias = null)
        {
            if (string.IsNullOrWhiteSpace(_filter))
                return true;
            try
            {
                JArray ja = JArray.Parse(_filter);
                if (ja == null || !ja.Any())
                    return false;

                string predicate = string.Empty;

                for (int i = 0, j = i; i < ja.Count; i++)
                {
                    var jt = ja[i];
                    if (!jt.HasValues)
                        continue;
                    if ((jt.Value<string>("group") ?? string.Empty) == "start")
                        predicate += "(";

                    string field = jt.Value<string>("field");
                    if (alias != null)
                        field = $"{alias}\"{field}\"";
                    string value = jt.Value<string>("value");

                    bool skip = false;
                    switch (jt.Value<string>("compare"))
                    {
                        case "in":
                            if (string.IsNullOrEmpty(value))
                            {
                                skip = true;
                                break;
                            }
                            predicate += $"{field} LIKE '{value}'";
                            break;
                        case "bin":
                            if (string.IsNullOrEmpty(value))
                            {
                                skip = true;
                                break;
                            }
                            predicate += $"'{value}' LIKE CONCAT('%',{field},'%')";
                            break;
                        case "eq":
                            predicate += $"{field} = '{value}'";
                            break;
                        case "ne":
                            predicate += $"{field} != '{value}'";
                            break;
                        case "le":
                            predicate += $"{field} <= '{value}'";
                            break;
                        case "lt":
                            predicate += $"{field} < '{value}'";
                            break;
                        case "ge":
                            predicate += $"{field} >= '{value}'";
                            break;
                        case "gt":
                            predicate += $"{field} > '{value}'";
                            break;
                        case "sin":
                            predicate += $"{field} IN ({value})";
                            break;
                        case "nsin":
                            predicate += $"{field} NOT IN ({value})";
                            break;
                        default:
                            skip = true;
                            break;
                    }


                    if ((jt.Value<string>("group") ?? string.Empty) == "end")
                        predicate += ")";

                    if (!skip && i != ja.Count - 1)
                    {
                        string relation = jt.Value<string>("relation");
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
        /// <param name="sql">sql语句</param>
        /// <param name="alias">别名</param>
        /// <returns>排序条件是否有误</returns>
        public bool OrderByToSql(ref string sql, string alias = null)
        {
            try
            {
                string predicate = string.Empty;

                if (!string.IsNullOrEmpty(AdvancedSort))
                {
                    JArray ja = JArray.Parse(AdvancedSort);
                    if (ja == null || !ja.Any())
                        return false;

                    predicate = " ";

                    for (int i = 0, j = i; i < ja.Count; i++)
                    {
                        var jt = ja[i];
                        if (!jt.HasValues)
                            continue;

                        string field = jt.Value<string>("field");
                        if (alias != null)
                            field = $"{alias}.\"{field}\"";
                        string type = jt.Value<string>("type");


                        if (string.IsNullOrEmpty(type))
                            type = "asc";

                        predicate += $"{field} {type}";
                    }
                }
                else if (!string.IsNullOrEmpty(SortField))
                {
                    if (alias != null)
                        SortField = $"{alias}.\"{SortField}\"";
                    predicate += $" {SortField} {(string.IsNullOrEmpty(SortType) ? "asc" : SortType)}";
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
        /// <param name="LinqDynamic">动态linq</param>
        /// <returns>筛选条件是否有误</returns>
        public bool FilterToLinqDynamic<TSource>(ref IQueryable<TSource> LinqDynamic)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_filter))
                    return true;

                JArray ja = JArray.Parse(_filter);
                if (ja == null || !ja.Any())
                    return false;

                string predicate = string.Empty;
                List<object> args = new List<object>();

                for (int i = 0, j = i; i < ja.Count; i++)
                {
                    var jt = ja[i];
                    if (!jt.HasValues)
                        continue;
                    if ((jt.Value<string>("group") ?? string.Empty) == "start")
                        predicate += "(";

                    string field = jt.Value<string>("field");
                    string value = jt.Value<string>("value");
                    bool skip = false;
                    switch (jt.Value<string>("compare"))
                    {
                        case "in":
                            if (string.IsNullOrEmpty(value))
                            {
                                skip = true;
                                break;
                            }
                            predicate += $"{field}.Contains(@{j})";
                            args.Add(value);
                            break;
                        case "bin":
                            if (string.IsNullOrEmpty(value))
                            {
                                skip = true;
                                break;
                            }
                            predicate += $"@{j}.Contains({field})";
                            args.Add(value);
                            break;
                        case "eq":
                            predicate += $"{field} == @{j}";
                            args.Add(value);
                            break;
                        case "ne":
                            predicate += $"!{field}.Equals(@{j})";
                            args.Add(value);
                            break;
                        case "le":
                            predicate += $"{field} <= @{j}";
                            args.Add(value);
                            break;
                        case "lt":
                            predicate += $"{field} < @{j}";
                            args.Add(value);
                            break;
                        case "ge":
                            predicate += $"{field} >= @{j}";
                            args.Add(value);
                            break;
                        case "gt":
                            predicate += $"{field} > @{j}";
                            args.Add(value);
                            break;
                        default:
                            skip = true;
                            break;
                    }


                    if ((jt.Value<string>("group") ?? string.Empty) == "end")
                        predicate += ")";

                    if (!skip && i != ja.Count - 1)
                    {
                        string relation = jt.Value<string>("relation");
                        switch (relation)
                        {
                            case "or":
                                predicate += $" || ";
                                break;
                            case "and":
                            default:
                                predicate += $" && ";
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
        /// <typeparam name="TSource"></typeparam>
        /// <param name="LinqDynamic">动态linq</param>
        /// <returns>排序条件是否有误</returns>
        public bool OrderByToLinqDynamic<TSource>(ref IQueryable<TSource> LinqDynamic)
        {
            try
            {
                if (!string.IsNullOrEmpty(AdvancedSort))
                {
                    JArray ja = JArray.Parse(AdvancedSort);
                    if (ja == null || !ja.Any())
                        return false;

                    IOrderedQueryable<TSource> orderByLinq = null;
                    for (int i = 0, j = i; i < ja.Count; i++)
                    {
                        var jt = ja[i];
                        if (!jt.HasValues)
                            continue;

                        string field = jt.Value<string>("field");
                        string type = jt.Value<string>("type");


                        if (string.IsNullOrEmpty(type))
                            type = "asc";

                        if (orderByLinq == null)
                            orderByLinq = LinqDynamic.OrderBy($"{field} {type}");
                        else
                            orderByLinq = orderByLinq.ThenBy($"{field} {type}");
                    }

                    LinqDynamic = orderByLinq;
                }
                else if (!string.IsNullOrEmpty(SortField))
                {
                    LinqDynamic = LinqDynamic.OrderBy($"{SortField} {(string.IsNullOrEmpty(SortType) ? "asc" : SortType)}");
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        private long _recordCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        private long _pageCount
        {
            get
            {
                long pages = _recordCount / _pageRows;
                long pageCount = _recordCount % _pageRows == 0 ? pages : pages + 1;
                return pages;
            }
        }

        private Stopwatch _watch { get; set; } = new Stopwatch();

        #endregion

        #region 输入

        #region 默认方案

        /// <summary>
        /// 当前页码
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType._integer, OpenApiSchemaFormat.integer_int32, 1)]
        [OpenApiSubTag("Pagination")]
        public int PageIndex { get => _pageIndex; set => _pageIndex = value; }

        /// <summary>
        /// 每页数据量
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType._integer, OpenApiSchemaFormat.integer_int32, 10)]
        [OpenApiSubTag("Pagination")]
        public int PageRows { get => _pageRows; set => _pageRows = value; }

        /// <summary>
        /// 排序列
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType._string, Value = "CreateTime")]
        [OpenApiSubTag("Pagination")]
        public string SortField { get => _sortField; set => _sortField = value; }

        /// <summary>
        /// 排序类型
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType._string, Value = "desc")]
        [OpenApiSubTag("Pagination")]
        public string SortType { get => _sortType; set => _sortType = value; }

        /// <summary>
        /// 高级排序
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType._string)]
        [OpenApiSubTag("Pagination")]
        public string AdvancedSort { get => _advancedSort; set => _advancedSort = value; }

        /// <summary>
        /// 筛选条件
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType._string)]
        [OpenApiSubTag("Pagination")]
        public string Filter { get => _filter; set => _filter = value; }

        /// <summary>
        /// 架构
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType._string, null, "defaul")]
        [OpenApiSubTag("Pagination")]
        public Schema Schema { get => _schema; set => _schema = value; }

        /// <summary>
        /// 总记录数
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType._integer, OpenApiSchemaFormat.integer_int64)]
        public long RecordCount { get => _recordCount; set => _recordCount = value; }

        /// <summary>
        /// 总页数
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType._integer, OpenApiSchemaFormat.integer_int64)]
        public long PageCount { get => _pageCount; }

        #endregion

        #region jqGrid方案

        /// <summary>
        /// 当前页
        /// </summary>
        public int page { get => _pageIndex; set => _pageIndex = value; }

        /// <summary>
        /// 每页数据量
        /// </summary>
        public int rows { get => _pageRows; set => _pageRows = value; }

        /// <summary>
        /// 排序列
        /// </summary>
        public string sidx { get => _sortField; set => _sortField = value; }

        /// <summary>
        /// 排序类型
        /// </summary>
        public string sord { get => _sortType; set => _sortType = value; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public long records { get => _recordCount; set => _recordCount = value; }

        /// <summary>
        /// 总页数
        /// </summary>
        public long total { get => _pageCount; }

        #endregion

        #region layui方案

        public int limit { get => _pageRows; set => _pageRows = value; }

        #endregion

        #region Easyui DataGrid方案

        /// <summary>
        /// 排序列
        /// </summary>
        public string sort { get => _sortField; set => _sortField = value; }

        /// <summary>
        /// 排序类型
        /// </summary>
        public string order { get => _sortType; set => _sortType = value; }

        #endregion

        #region BootstrapTable方案

        public int offset { get => (_pageIndex - 1) * limit; set => _pageIndex = value / limit + 1; }

        #endregion

        #endregion

        #region 输出

        /// <summary>
        /// 输出
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="success">成功与否</param>
        /// <param name="error">异常信息</param>
        /// <returns></returns>
        public object BuildResult(object data, bool success = true, string error = null)
        {
            _watch.Stop();
            switch (_schema)
            {
                case Schema.defaul:
                case Schema.elementVue:
                    return new AjaxResult<object>()
                    {
                        Success = success,
                        ErrorCode = (int)(success ? ErrorCode.none : ErrorCode.error),
                        Msg = error,
                        Data = new
                        {
                            PageTotal = PageCount,
                            Total = RecordCount,
                            PageIndex,
                            PageSize = PageRows,
                            List = data
                        }
                    };
                case Schema.antdVue:
                    return new
                    {
                        Success = success,
                        Msg = error,
                        Total = RecordCount,
                        Data = data
                    };
                case Schema.jqGrid:
                    return new Result.JqGrid()
                    {
                        rows = data,
                        total = PageCount,
                        page = PageIndex,
                        records = RecordCount,
                        costtime = _watch.ElapsedMilliseconds
                    };
                case Schema.easyui:
                case Schema.bootstrapTable:
                    return new Result.Easyui()
                    {
                        rows = data,
                        total = RecordCount,
                        currentPage = PageIndex,
                        pageSize = PageRows,
                        costtime = _watch.ElapsedMilliseconds
                    };
                case Schema.layui:
                default:
                    return new Result.Layui()
                    {
                        data = data,
                        code = success ? 0 : -1,
                        msg = error,
                        count = RecordCount,
                        costtime = _watch.ElapsedMilliseconds
                    };
            }
        }

        /// <summary>
        /// 返回信息
        /// </summary>
        public class Result
        {
            /// <summary>
            /// Layui方案
            /// </summary>
            public class Layui
            {
                /// <summary>
                /// 数据
                /// </summary>
                public object data { get; set; }

                /// <summary>
                /// 状态码
                /// <para>成功0，失败-1</para>
                /// </summary>
                public int code { get; set; }

                /// <summary>
                /// 信息
                /// </summary>
                public string msg { get; set; }

                /// <summary>
                /// 总记录数
                /// </summary>
                public long count { get; set; }

                /// <summary>
                /// 耗时（毫秒）
                /// </summary>
                public long costtime { get; set; }
            }

            /// <summary>
            /// JqGrid方案
            /// </summary>
            public class JqGrid
            {
                /// <summary>
                /// 数据
                /// </summary>
                public object rows { get; set; }

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

            /// <summary>
            /// Easyui DataGrid方案
            /// </summary>
            public class Easyui
            {
                /// <summary>
                /// 数据
                /// </summary>
                public object rows { get; set; }

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

        #endregion
    }



    /// <summary>
    /// 架构
    /// </summary>
    public enum Schema
    {
        /// <summary>
        /// 默认
        /// </summary>
        defaul,
        /// <summary>
        /// layui
        /// <para>https://www.layui.com/doc/modules/table.html#response</para>
        /// </summary>
        layui,
        /// <summary>
        /// jqGrid
        /// <para>https://blog.mn886.net/jqGrid/api/jsondata/index.jsp</para>
        /// </summary>
        jqGrid,
        /// <summary>
        /// easyui
        /// <para>http://www.jeasyui.net/plugins/183.html</para>
        /// </summary>
        easyui,
        /// <summary>
        /// bootstrapTable
        /// <para>https://bootstrap-table.com/docs/api/table-options/</para>
        /// </summary>
        bootstrapTable,
        /// <summary>
        /// Ant Design + Vue
        /// <para>https://www.antdv.com/components/list-cn/#api</para>
        /// </summary>
        antdVue,
        /// <summary>
        /// element + Vue
        /// </summary>
        elementVue,
    }
}
