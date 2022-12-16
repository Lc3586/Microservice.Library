using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.Extension.Model
{
    /// <summary>
    /// 筛选
    /// <para>LCTR 2019-06-04</para>
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// 分组
        /// </summary>
        public GroupFilter Group { get; set; }

        /// <summary>
        /// 条件
        /// </summary>
        public FilterParam Param { get; set; }
    }

    /// <summary>
    /// 分组
    /// </summary>
    public class GroupFilter
    {
        /// <summary>
        /// true:AND,false:OR
        /// </summary>
        public bool AndOr { get; set; }

        /// <summary>
        /// 筛选
        /// </summary>
        public List<Filter> Filter { get; set; }
    }

    /// <summary>
    /// 条件
    /// </summary>
    public class FilterParam
    {
        /// <summary>
        /// 字段
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 比较方式
        /// </summary>
        public CompareType Compare { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }
    }

    /// <summary>
    /// 比较类型
    /// </summary>
    public enum CompareType
    {
        /// <summary>
        /// 包含
        /// </summary>
        IN,
        /// <summary>
        /// 被包含
        /// </summary>
        BIN,
        /// <summary>
        /// 包含
        /// <para>集合</para>
        /// </summary>
        SIN,
        /// <summary>
        /// 不包含
        /// <para>集合</para>
        /// </summary>
        NSIN,
        /// <summary>
        /// 相等
        /// </summary>
        EQ,
        /// <summary>
        /// 不相等
        /// </summary>
        NE,
        /// <summary>
        /// 小于等于
        /// </summary>
        LE,
        /// <summary>
        /// 小于
        /// </summary>
        LT,
        /// <summary>
        /// 大于等于
        /// </summary>
        GE,
        /// <summary>
        /// 大于
        /// </summary>
        GT
    }
}
