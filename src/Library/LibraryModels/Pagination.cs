using Library.OpenApi.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            _sortType = SortType.desc;
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
        private SortType _sortType { get; set; }

        /// <summary>
        /// 高级排序
        /// </summary>
        private List<PaginationAdvancedSort> _advancedSort { get; set; }

        /// <summary>
        /// 筛选条件
        /// </summary>
        private List<PaginationFilter> _filter { get; set; }

        /// <summary>
        /// 筛选条件转sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="alias">别名</param>
        /// <returns>筛选条件是否有误</returns>
        public bool FilterToSql(ref string sql, string alias = null)
        {
            if (_filter == null || !_filter.Any())
                return true;
            try
            {
                string predicate = string.Empty;

                for (int i = 0, j = i; i < _filter.Count; i++)
                {
                    var filter = _filter[i];
                    if (filter == null)
                        continue;

                    if (filter.Group?.Flag == FilterGroupFlag.start)
                        predicate += "(";

                    string field = filter.Field;
                    if (alias != null)
                        field = $"{alias}.\"{field}\"";
                    string value = filter.Value.ToString();

                    bool skip = false;
                    switch (filter.Compare)
                    {
                        case FilterCompare.@in:
                            if (string.IsNullOrEmpty(value))
                            {
                                skip = true;
                                break;
                            }
                            predicate += $"{field} LIKE '{value}'";
                            break;
                        case FilterCompare.bin:
                            if (string.IsNullOrEmpty(value))
                            {
                                skip = true;
                                break;
                            }
                            predicate += $"'{value}' LIKE CONCAT('%',{field},'%')";
                            break;
                        case FilterCompare.eq:
                            predicate += $"{field} = '{value}'";
                            break;
                        case FilterCompare.ne:
                            predicate += $"{field} != '{value}'";
                            break;
                        case FilterCompare.le:
                            predicate += $"{field} <= '{value}'";
                            break;
                        case FilterCompare.lt:
                            predicate += $"{field} < '{value}'";
                            break;
                        case FilterCompare.ge:
                            predicate += $"{field} >= '{value}'";
                            break;
                        case FilterCompare.gt:
                            predicate += $"{field} > '{value}'";
                            break;
                        case FilterCompare.sin:
                            predicate += $"{field} IN ({value})";
                            break;
                        case FilterCompare.nsin:
                            predicate += $"{field} NOT IN ({value})";
                            break;
                        default:
                            skip = true;
                            break;
                    }

                    if (filter.Group?.Flag == FilterGroupFlag.end)
                        predicate += ")";

                    if (!skip && i != _filter.Count - 1)
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
        /// <param name="sql">sql语句</param>
        /// <param name="alias">别名</param>
        /// <returns>排序条件是否有误</returns>
        public bool OrderByToSql(ref string sql, string alias = null)
        {
            try
            {
                string predicate = string.Empty;

                if (AdvancedSort != null && AdvancedSort.Any())
                {
                    predicate = " ";

                    foreach (var item in AdvancedSort)
                    {
                        if (item == null)
                            continue;

                        string field = item.Field;
                        if (alias != null)
                            field = $"{alias}.\"{field}\"";
                        string type = item.Type.ToString();


                        if (string.IsNullOrEmpty(type))
                            type = "asc";

                        predicate += $"{field} {type}";
                    }
                }
                else if (!string.IsNullOrEmpty(SortField))
                {
                    if (alias != null)
                        SortField = $"{alias}.\"{SortField}\"";
                    predicate += $" {SortField} {SortType}";
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
                if (_filter == null || !_filter.Any())
                    return true;

                string predicate = string.Empty;
                List<object> args = new List<object>();

                for (int i = 0, j = i; i < _filter.Count; i++)
                {
                    var filter = _filter[i];
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
                            predicate += $"{field}.Contains(@{j})";
                            args.Add(value);
                            break;
                        case FilterCompare.bin:
                            if (string.IsNullOrEmpty(value.ToString()))
                            {
                                skip = true;
                                break;
                            }
                            predicate += $"@{j}.Contains({field})";
                            args.Add(value);
                            break;
                        case FilterCompare.eq:
                            predicate += $"{field} == @{j}";
                            args.Add(value);
                            break;
                        case FilterCompare.ne:
                            predicate += $"!{field}.Equals(@{j})";
                            args.Add(value);
                            break;
                        case FilterCompare.le:
                            predicate += $"{field} <= @{j}";
                            args.Add(value);
                            break;
                        case FilterCompare.lt:
                            predicate += $"{field} < @{j}";
                            args.Add(value);
                            break;
                        case FilterCompare.ge:
                            predicate += $"{field} >= @{j}";
                            args.Add(value);
                            break;
                        case FilterCompare.gt:
                            predicate += $"{field} > @{j}";
                            args.Add(value);
                            break;
                        case FilterCompare.sin:
                        case FilterCompare.nsin:
                        default:
                            skip = true;
                            break;
                    }

                    if (filter.Group?.Flag == FilterGroupFlag.end)
                        predicate += ")";

                    if (!skip && i != _filter.Count - 1)
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
        /// <typeparam name="TSource"></typeparam>
        /// <param name="LinqDynamic">动态linq</param>
        /// <returns>排序条件是否有误</returns>
        public bool OrderByToLinqDynamic<TSource>(ref IQueryable<TSource> LinqDynamic)
        {
            try
            {
                if (AdvancedSort != null && AdvancedSort.Any())
                {
                    IOrderedQueryable<TSource> orderByLinq = null;

                    foreach (var item in AdvancedSort)
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
                else if (!string.IsNullOrEmpty(SortField))
                {
                    LinqDynamic = LinqDynamic.OrderBy($"{SortField} {SortType}");
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
                return pageCount;
            }
        }

        private Stopwatch _watch { get; set; } = new Stopwatch();

        #endregion

        #region 输入

        #region 默认方案

        /// <summary>
        /// 当前页码
        /// <para>默认值 1</para>
        /// <para>值为-1时表示不分页</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.integer, OpenApiSchemaFormat.integer_int32, 1)]
        public int PageIndex { get => _pageIndex; set => _pageIndex = value; }

        /// <summary>
        /// 每页数据量
        /// <para>默认值 50</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.integer, OpenApiSchemaFormat.integer_int32, 50)]
        public int PageRows { get => _pageRows; set => _pageRows = value; }

        /// <summary>
        /// 排序列
        /// <para>默认值 Id</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@string, Value = "Id")]
        public string SortField { get => _sortField; set => _sortField = value; }

        /// <summary>
        /// 排序类型
        /// <para>默认值 desc</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@enum, OpenApiSchemaFormat.enum_description, Value = SortType.desc)]
        [JsonConverter(typeof(StringEnumConverter))]
        public SortType SortType { get => _sortType; set => _sortType = value; }

        /// <summary>
        /// 高级排序
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<PaginationAdvancedSort> AdvancedSort { get => _advancedSort; set => _advancedSort = value; }

        /// <summary>
        /// 筛选条件
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<PaginationFilter> Filter { get => _filter; set => _filter = value; }

        /// <summary>
        /// 架构
        /// <para>默认值 defaul</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@enum, OpenApiSchemaFormat.enum_description, Schema.defaul)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Schema Schema { get => _schema; set => _schema = value; }

        /// <summary>
        /// 总记录数
        /// </summary>
        //[OpenApiSchema(OpenApiSchemaType.integer, OpenApiSchemaFormat.integer_int64)]
        public long RecordCount { get => _recordCount; set => _recordCount = value; }

        /// <summary>
        /// 总页数
        /// </summary>
        //[OpenApiSchema(OpenApiSchemaType.integer, OpenApiSchemaFormat.integer_int64)]
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
        public SortType sord { get => _sortType; set => _sortType = value; }

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
        public SortType order { get => _sortType; set => _sortType = value; }

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
                    return new
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
    /// 分页筛选条件
    /// </summary>
    [OpenApiSchemaStrictMode]
    [OpenApiMainTag("PaginationFilter")]
    public class PaginationFilter
    {
        /// <summary>
        /// 要比较的字段
        /// <para>注意区分大小写</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@string)]
        public string Field { get; set; }

        /// <summary>
        /// 用于比较的值
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@string)]
        public object Value { get; set; }

        /// <summary>
        /// 比较类型
        /// <para>默认值 eq</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@enum, OpenApiSchemaFormat.enum_description, FilterCompare.eq)]
        [JsonConverter(typeof(StringEnumConverter))]
        public FilterCompare Compare { get; set; } = FilterCompare.eq;

        /// <summary>
        /// 分组设置
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public FilterGroupSetting Group { get; set; }
    }

    /// <summary>
    /// 分组设置
    /// </summary>
    [OpenApiSchemaStrictMode]
    [OpenApiMainTag("FilterGroupSetting")]
    public class FilterGroupSetting
    {
        /// <summary>
        /// 分组标识
        /// <para>默认值 keep</para>
        /// <para>用于标识分组的开始和结束</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@enum, OpenApiSchemaFormat.enum_description, FilterGroupFlag.keep)]
        [JsonConverter(typeof(StringEnumConverter))]
        public FilterGroupFlag Flag { get; set; } = FilterGroupFlag.keep;

        /// <summary>
        /// 组内关系
        /// <para>默认值 and</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@enum, OpenApiSchemaFormat.enum_description, FilterGroupRelation.and)]
        [JsonConverter(typeof(StringEnumConverter))]
        public FilterGroupRelation Relation { get; set; } = FilterGroupRelation.and;
    }

    /// <summary>
    /// 分页高级排序
    /// </summary>
    [OpenApiSchemaStrictMode]
    [OpenApiMainTag("PaginationAdvancedSort")]
    public class PaginationAdvancedSort
    {
        /// <summary>
        /// 字段
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@string)]
        public string Field { get; set; }

        /// <summary>
        /// 类型
        /// <para>默认值 desc</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@enum, OpenApiSchemaFormat.enum_description, SortType.desc)]
        [JsonConverter(typeof(StringEnumConverter))]
        public SortType Type { get; set; } = SortType.desc;
    }

    /// <summary>
    /// 筛选条件比较类型
    /// <para>in / 0 (包含)</para>
    /// <para>bin / 1 (被包含)</para>
    /// <para>eq / 2 (相等)</para>
    /// <para>ne / 3 (不相等)</para>
    /// <para>le / 4 (小于等于)</para>
    /// <para>lt / 5 (小于)</para>
    /// <para>ge / 6 (大于等于)</para>
    /// <para>gt / 7 (大于)</para>
    /// <para>sin / 8 (在集合中 <para>,号分隔</para><para>值不是数字的情况下需要用单引号将值括起来</para><para>完整示例:(1, 2, 3)、('A', 'B', 'C')</para>)</para>
    /// <para>nsin / 9 (不在集合中 <para>,号分隔</para><para>值不是数字的情况下需要用单引号将值括起来</para><para>完整示例:(1, 2, 3)、('A', 'B', 'C')</para>)</para>
    /// </summary>
    public enum FilterCompare
    {
        /// <summary>
        /// 包含
        /// </summary>
        [Description("包含")]
        @in,
        /// <summary>
        /// 被包含
        /// </summary>
        [Description("被包含")]
        bin,
        /// <summary>
        /// 相等
        /// </summary>
        [Description("相等")]
        eq,
        /// <summary>
        /// 不相等
        /// </summary>
        [Description("不相等")]
        ne,
        /// <summary>
        /// 小于等于
        /// </summary>
        [Description("小于等于")]
        le,
        /// <summary>
        /// 小于
        /// </summary>
        [Description("小于")]
        lt,
        /// <summary>
        /// 大于等于
        /// </summary>
        [Description("大于等于")]
        ge,
        /// <summary>
        /// 大于
        /// </summary>
        [Description("大于")]
        gt,
        /// <summary>
        /// 在集合中
        /// <para>,号分隔</para>
        /// <para>值不是数字的情况下需要用单引号将值括起来</para>
        /// <para>完整示例:(1, 2, 3)、('A', 'B', 'C')</para>
        /// </summary>
        [Description("在集合中")]
        sin,
        /// <summary>
        /// 不在集合中
        /// <para>,号分隔</para>
        /// <para>值不是数字的情况下需要用单引号将值括起来</para>
        /// <para>完整示例:(1, 2, 3)、('A', 'B', 'C')</para>
        /// </summary>
        [Description("不在集合中")]
        nsin
    }

    /// <summary>
    /// 筛选条件分组标识
    /// <para>start / 0 (开始)</para>
    /// <para>keep / 1 (还在分组内)</para>
    /// <para>end / 2 (结束)</para>
    /// </summary>
    public enum FilterGroupFlag
    {
        /// <summary>
        /// 开始
        /// </summary>
        [Description("开始")]
        start,
        /// <summary>
        /// 还在分组内
        /// </summary>
        [Description("还在分组内")]
        keep,
        /// <summary>
        /// 结束
        /// </summary>
        [Description("结束")]
        end
    }

    /// <summary>
    /// 筛选条件分组关系类型
    /// <para>and / 0 (并且)</para>
    /// <para>or / 1 (或)</para>
    /// </summary>
    public enum FilterGroupRelation
    {
        /// <summary>
        /// 并且
        /// </summary>
        [Description("并且")]
        and,
        /// <summary>
        /// 或
        /// </summary>
        [Description("或")]
        or
    }

    /// <summary>
    /// 排序类型
    /// <para>asc / 0 (正序)</para>
    /// <para>desc / 1 (倒序)</para>
    /// </summary>
    public enum SortType
    {
        /// <summary>
        /// 正序
        /// </summary>
        [Description("正序")]
        asc,
        /// <summary>
        /// 倒序
        /// </summary>
        [Description("倒序")]
        desc
    }

    /// <summary>
    /// 架构
    /// <para>defaul / 0 (默认)</para>
    /// <para>layui / 1 (layui <para>https://www.layui.com/doc/modules/table.html#response</para>)</para>
    /// <para>jqGrid / 2 (jqGrid <para>https://blog.mn886.net/jqGrid/api/jsondata/index.jsp</para>)</para>
    /// <para>easyui / 3 (easyui <para>http://www.jeasyui.net/plugins/183.html</para>)</para>
    /// <para>bootstrapTable / 4 (bootstrapTable <para>https://bootstrap-table.com/docs/api/table-options/</para>)</para>
    /// <para>antdVue / 5 (Ant Design + Vue <para>https://www.antdv.com/components/list-cn/#api</para>)</para>
    /// <para>elementVue / 6 (element + Vue)</para>
    /// </summary>
    public enum Schema
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("默认")]
        defaul,
        /// <summary>
        /// layui
        /// <para>https://www.layui.com/doc/modules/table.html#response</para>
        /// </summary>
        [Description("layui")]
        layui,
        /// <summary>
        /// jqGrid
        /// <para>https://blog.mn886.net/jqGrid/api/jsondata/index.jsp</para>
        /// </summary>
        [Description("jqGrid")]
        jqGrid,
        /// <summary>
        /// easyui
        /// <para>http://www.jeasyui.net/plugins/183.html</para>
        /// </summary>
        [Description("easyui")]
        easyui,
        /// <summary>
        /// bootstrapTable
        /// <para>https://bootstrap-table.com/docs/api/table-options/</para>
        /// </summary>
        [Description("bootstrapTable")]
        bootstrapTable,
        /// <summary>
        /// Ant Design + Vue
        /// <para>https://www.antdv.com/components/list-cn/#api</para>
        /// </summary>
        [Description("Ant Design + Vue")]
        antdVue,
        /// <summary>
        /// element + Vue
        /// </summary>
        [Description("element + Vue")]
        elementVue,
    }
}
