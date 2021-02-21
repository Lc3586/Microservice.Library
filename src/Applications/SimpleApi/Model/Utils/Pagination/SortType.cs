using System.ComponentModel;

namespace Model.Utils.Pagination
{
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
}
