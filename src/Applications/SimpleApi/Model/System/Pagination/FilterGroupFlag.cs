using System.ComponentModel;

namespace Model.System.Pagination
{
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
}
