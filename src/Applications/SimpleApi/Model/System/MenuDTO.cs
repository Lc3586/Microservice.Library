using Entity.Example;
using Entity.System;
using Library.DataMapping.Annotations;
using Library.DataMapping.Application;
using Library.OpenApi.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
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
        /// 角色类型
        /// </summary>
        public List<string> RoleType { get; set; }
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
    /// 基础信息
    /// </summary>
    [MapFrom(typeof(System_Menu))]
    [OpenApiMainTag("Base")]
    public class Base : System_Menu
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
    [MapTo(typeof(System_Menu))]
    [OpenApiMainTag("Sort")]
    public class Sort : System_Menu
    {
        /// <summary>
        /// 排序类型
        /// <para>默认值 up</para>
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@enum, OpenApiSchemaFormat.enum_description, SortType.up)]
        [JsonConverter(typeof(StringEnumConverter))]
        public SortType SortType { get; set; }

        /// <summary>
        /// 跨度
        /// <para>移动几位</para>
        /// <para>默认值 1</para>
        /// </summary>
        public int Span { get; set; } = 1;
    }
}
