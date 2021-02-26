using Entity.System;
using Microservice.Library.DataMapping.Annotations;
using Microservice.Library.OpenApi.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel;

/* 
 * 菜单业务模型
 */
namespace Model.System.MenuDTO
{
    /// <summary>
    /// 列表
    /// </summary>
    [MapFrom(typeof(System_Menu))]
    [OpenApiMainTag("List")]
    public class List : System_Menu
    {

    }

    /// <summary>
    /// 树状列表参数
    /// </summary>
    public class TreeListParamter
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 层级数
        /// <para>为空则表示获取所有层级数据</para>
        /// </summary>
        public int? Rank { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        public List<string> MenuType { get; set; }
    }

    /// <summary>
    /// 树状列表
    /// </summary>
    [MapFrom(typeof(System_Menu))]
    [OpenApiMainTag("TreeList")]
    public class TreeList : System_Menu
    {
        /// <summary>
        /// 子级菜单
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model, OpenApiSchemaFormat.model_once)]
        [Description("子级菜单")]
        public List<TreeList> Childs_ { get; set; }
    }

    /// <summary>
    /// 详情
    /// </summary>
    [MapFrom(typeof(System_Menu))]
    [OpenApiMainTag("Detail")]
    public class Detail : System_Menu
    {

    }

    /// <summary>
    /// 授权信息
    /// </summary>
    [MapFrom(typeof(System_Menu))]
    [OpenApiMainTag("Authorities")]
    public class Authorities : System_Menu
    {

    }

    /// <summary>
    /// 新增
    /// </summary>
    [MapTo(typeof(System_Menu))]
    [OpenApiMainTag("Create")]
    public class Create : System_Menu
    {

    }

    /// <summary>
    /// 编辑
    /// </summary>
    [MapFrom(typeof(System_Menu))]
    [MapTo(typeof(System_Menu))]
    [OpenApiMainTag("Edit")]
    public class Edit : System_Menu
    {

    }

    /// <summary>
    /// 排序
    /// </summary>
    public class Sort
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 排序类型
        /// <para>默认值 up</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@enum, OpenApiSchemaFormat.enum_description, SortType.up)]
        [JsonConverter(typeof(StringEnumConverter))]
        public SortType Type { get; set; }

        /// <summary>
        /// 跨度
        /// <para>移动几位</para>
        /// <para>默认值 1</para>
        /// </summary>
        public int Span { get; set; } = 1;
    }

    /// <summary>
    /// 拖动排序
    /// </summary>
    public class DragSort
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 目标Id
        /// </summary>
        public string TargetId { get; set; }

        /// <summary>
        /// 是否位于目标后面
        /// </summary>
        public bool Append { get; set; }
    }
}
