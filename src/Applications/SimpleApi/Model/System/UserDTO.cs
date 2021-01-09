using Entity.Example;
using Entity.System;
using Library.DataMapping.Annotations;
using Library.DataMapping.Application;
using Library.OpenApi.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;

/* 
 * 用户业务模型
 */
namespace Model.System.UserDTO
{
    /// <summary>
    /// 列表
    /// </summary>
    [MapFrom(typeof(System_User))]
    [OpenApiMainTag("List")]
    public class List : System_User
    {

    }

    /// <summary>
    /// 详情
    /// </summary>
    [MapFrom(typeof(System_User))]
    [OpenApiMainTag("Detail")]
    public class Detail : System_User
    {

    }

    /// <summary>
    /// 授权信息
    /// </summary>
    [MapFrom(typeof(System_User))]
    [OpenApiMainTag("Authorities")]
    public class Authorities : System_User
    {
        /// <summary>
        /// 授权给此用户的角色
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        [Description("授权给此用户的角色")]
        public List<RoleDTO.Authorities> _Roles { get; set; }

        /// <summary>
        /// 授权给此用户的菜单
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        [Description("直接授权给此用户的菜单")]
        public List<MenuDTO.Authorities> _Menus { get; set; }


        /// <summary>
        /// 授权给此用户的资源
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        [Description("直接授权给此用户的资源")]
        public List<ResourcesDTO.Authorities> _Resources { get; set; }
    }

    /// <summary>
    /// 新增
    /// </summary>
    [MapTo(typeof(System_User))]
    [OpenApiMainTag("Create")]
    public class Create : System_User
    {

    }

    /// <summary>
    /// 编辑
    /// </summary>
    [MapFrom(typeof(System_User))]
    [MapTo(typeof(System_User))]
    [OpenApiMainTag("Edit")]
    public class Edit : System_User
    {

    }
}
