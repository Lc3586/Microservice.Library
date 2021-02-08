using System.ComponentModel;

namespace Model.System.Pagination
{
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
}
