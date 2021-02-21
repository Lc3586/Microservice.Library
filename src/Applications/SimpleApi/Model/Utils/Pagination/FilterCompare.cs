using System;
using System.ComponentModel;

namespace Model.Utils.Pagination
{
    /// <summary>
    /// 筛选条件比较类型
    /// <para>in / 0 (包含)</para>
    /// <para>inStart / 1 (前段包含)</para>
    /// <para>inEnd / 2 (后段包含)</para>
    /// <para>includedIn / 3 (包含于)</para>
    /// <para>notIn / 4 (不包含)</para>
    /// <para>notInStart / 5 (前段不包含)</para>
    /// <para>notInEnd / 6 (后段不包含)</para>
    /// <para>notIncludedIn / 7 (不包含于)</para>
    /// <para>eq / 8 (相等)</para>
    /// <para>notEq / 9 (不相等)</para>
    /// <para>le / 10 (小于等于)</para>
    /// <para>lt / 11 (小于)</para>
    /// <para>ge / 12 (大于等于)</para>
    /// <para>gt / 13 (大于)</para>
    /// <para>inSet / 14 (在集合中 <para>,号分隔</para><para>示例:1, 2, 3</para>)</para>
    /// <para>notInSet / 15 (不在集合中 <para>,号分隔</para><para>示例:1, 2, 3</para>)</para>
    /// <para>range / 16 (范围)</para>
    /// <para>dateRange / 17 (日期范围)</para>
    /// </summary>
    public enum FilterCompare
    {
        /// <summary>
        /// 包含（'%Value%'）
        /// </summary>
        [Description("包含")]
        @in,
        /// <summary>
        /// 前段包含（'Value%'）
        /// </summary>
        [Description("前段包含")]
        inStart,
        /// <summary>
        /// 后段包含（'%Value'）
        /// </summary>
        [Description("后段包含")]
        inEnd,
        /// <summary>
        /// 包含于
        /// </summary>
        [Description("包含于")]
        [Obsolete("由于Freesql不支持，此功能暂弃用")]
        includedIn,
        /// <summary>
        /// 不包含（'%Value%'）
        /// </summary>
        [Description("不包含")]
        notIn,
        /// <summary>
        /// 前段不包含（'Value%'）
        /// </summary>
        [Description("前段不包含")]
        notInStart,
        /// <summary>
        /// 后段不包含（'%Value'）
        /// </summary>
        [Description("后段不包含")]
        notInEnd,
        /// <summary>
        /// 不包含于
        /// </summary>
        [Description("不包含于")]
        [Obsolete("由于Freesql不支持，此功能暂弃用")]
        notIncludedIn,
        /// <summary>
        /// 相等
        /// </summary>
        [Description("相等")]
        eq,
        /// <summary>
        /// 不相等
        /// </summary>
        [Description("不相等")]
        notEq,
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
        /// <para>示例:1, 2, 3</para>
        /// </summary>
        [Description("在集合中")]
        inSet,
        /// <summary>
        /// 不在集合中
        /// <para>,号分隔</para>
        /// <para>示例:1, 2, 3</para>
        /// </summary>
        [Description("不在集合中")]
        notInSet,
        /// <summary>
        /// 范围
        /// <para>Value1, Value2</para>
        /// </summary>
        [Description("范围")]
        range,
        /// <summary>
        /// 日期范围
        /// <para>date1, date2</para>
        /// <para>这是专门为日期范围查询定制的操作符，它会处理 date2 + 1，比如：</para>
        /// <para>当 date2 选择的是 2020-05-30，那查询的时候是 小于 2020-05-31</para>
        /// <para>当 date2 选择的是 2020-05，那查询的时候是 小于 2020-06</para>
        /// <para>当 date2 选择的是 2020，那查询的时候是 小于 2021</para>
        /// <para>当 date2 选择的是 2020-05-30 12，那查询的时候是 小于 2020-05-30 13</para>
        /// <para>当 date2 选择的是 2020-05-30 12:30，那查询的时候是 小于 2020-05-30 12:31</para>
        /// <para>并且 date2 只支持以上 5 种格式 (date1 没有限制)</para>
        /// </summary>
        [Description("日期范围")]
        dateRange
    }
}
