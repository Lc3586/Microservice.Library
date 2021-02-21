using System.ComponentModel;

namespace Model.Utils.Pagination
{
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
