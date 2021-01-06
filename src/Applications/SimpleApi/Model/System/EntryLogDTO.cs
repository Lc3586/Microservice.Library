using Entity.Example;
using Entity.System;
using Library.DataMapping.Annotations;
using Library.DataMapping.Application;
using Library.OpenApi.Annotations;
using System;
using System.ComponentModel;

/* 
 * 登录日志业务模型
 */
namespace Model.System.EntryLogDTO
{
    /// <summary>
    /// 列表
    /// </summary>
    [MapFrom(typeof(System_EntryLog))]
    [OpenApiMainTag("List")]
    public class List : System_EntryLog
    {

    }

    /// <summary>
    /// 详情
    /// </summary>
    [MapFrom(typeof(System_EntryLog))]
    [OpenApiMainTag("Detail")]
    public class Detail : System_EntryLog
    {
        /// <summary>
        /// 用户
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        [Description("用户详情")]
        public UserDTO.Detail _User { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        [Description("角色详情")]
        public RoleDTO.Detail _Role { get; set; }
    }
}
