using Entity.Example;
using Entity.System;
using Library.DataMapping.Annotations;
using Library.DataMapping.Application;
using Library.OpenApi.Annotations;
using System;
using System.ComponentModel;

/* 
 * 操作记录业务模型
 */
namespace Model.System.OperationRecordDTO
{
    /// <summary>
    /// 列表
    /// </summary>
    [MapFrom(typeof(System_OperationRecord))]
    [OpenApiMainTag("List")]
    public class List : System_OperationRecord
    {

    }

    /// <summary>
    /// 详情
    /// </summary>
    [MapFrom(typeof(System_OperationRecord))]
    [OpenApiMainTag("Detail")]
    public class Detail : System_OperationRecord
    {
        /// <summary>
        /// 用户详情
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        [Description("用户详情")]
        public UserDTO.Detail _User { get; set; }

        /// <summary>
        /// 角色详情
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        [Description("角色详情")]
        public RoleDTO.Detail _Role { get; set; }
    }
}
