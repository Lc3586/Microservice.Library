using Entity.Common;
using Library.DataMapping.Annotations;
using Library.OpenApi.Annotations;
using System.ComponentModel;

/* 
 * 登录日志业务模型
 */
namespace Model.Common.EntryLogDTO
{
    /// <summary>
    /// 列表
    /// </summary>
    [MapFrom(typeof(Common_EntryLog))]
    [OpenApiMainTag("List")]
    public class List : Common_EntryLog
    {

    }

    /// <summary>
    /// 详情
    /// </summary>
    [MapFrom(typeof(Common_EntryLog))]
    [OpenApiMainTag("Detail")]
    public class Detail : Common_EntryLog
    {
        /// <summary>
        /// 用户
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        [Description("用户详情")]
        public System.UserDTO.Detail _User { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        [Description("角色详情")]
        public System.RoleDTO.Detail _Role { get; set; }
    }
}
