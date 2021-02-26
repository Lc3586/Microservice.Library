using Microservice.Library.OpenApi.Annotations;
using Model.Utils.Result;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Model.Utils.Pagination
{
    /// <summary>
    /// 数据表格分页
    /// </summary>
    [OpenApiSchemaStrictMode]
    [OpenApiMainTag("Pagination")]
    public class PaginationDTO
    {
        #region 构造函数

        public PaginationDTO()
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
        /// 筛选条件
        /// </summary>
        private List<PaginationDynamicFilterInfo> _dynamicFilterInfo { get; set; }

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
        [Obsolete("建议使用DynamicFilterInfo")]
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<PaginationFilter> Filter { get => _filter; set => _filter = value; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 筛选条件
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<PaginationDynamicFilterInfo> DynamicFilterInfo { get => _dynamicFilterInfo; set => _dynamicFilterInfo = value; }

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
        /// <typeparam name="T">数据元素类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="success">成功与否</param>
        /// <param name="error">异常信息</param>
        /// <returns></returns>
        public object BuildResult<T>(List<T> data, bool success = true, string error = null)
        {
            _watch.Stop();
            switch (_schema)
            {
                case Schema.defaul:
                case Schema.elementVue:
                    return new ElementVueResult<T>()
                    {
                        Success = success,
                        ErrorCode = (int)(success ? ErrorCode.none : ErrorCode.error),
                        Msg = error,
                        Data = new ElementVueResultData<T>()
                        {
                            PageTotal = PageCount,
                            Total = RecordCount,
                            PageIndex = PageIndex,
                            PageSize = PageRows,
                            List = data
                        }
                    };
                case Schema.antdVue:
                    return new AntdVueResult<T>()
                    {
                        Success = success,
                        Msg = error,
                        Total = RecordCount,
                        Data = data
                    };
                case Schema.jqGrid:
                    return new JqGridResult<T>()
                    {
                        rows = data,
                        total = PageCount,
                        page = PageIndex,
                        records = RecordCount,
                        costtime = _watch.ElapsedMilliseconds
                    };
                case Schema.easyui:
                case Schema.bootstrapTable:
                    return new EasyuiResult<T>()
                    {
                        rows = data,
                        total = RecordCount,
                        currentPage = PageIndex,
                        pageSize = PageRows,
                        costtime = _watch.ElapsedMilliseconds
                    };
                case Schema.layui:
                default:
                    return new LayuiResult<T>()
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
        /// 获取方案的返回数据类型
        /// </summary>
        /// <typeparam name="T">数据元素类型</typeparam>
        /// <returns></returns>
        public Type GetSchemaResultType<T>()
        {
            return _schema switch
            {
                Schema.defaul => typeof(ElementVueResult<T>),
                Schema.elementVue => typeof(ElementVueResult<T>),
                Schema.antdVue => typeof(AntdVueResult<T>),
                Schema.jqGrid => typeof(JqGridResult<T>),
                Schema.easyui => typeof(EasyuiResult<T>),
                Schema.bootstrapTable => typeof(EasyuiResult<T>),
                Schema.layui => typeof(LayuiResult<T>),
                _ => typeof(LayuiResult<T>)
            };
        }

        #endregion
    }
}
