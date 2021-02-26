using Entity.Common;
using Microservice.Library.DataMapping.Annotations;
using Microservice.Library.OpenApi.Annotations;
using System.ComponentModel;

/* 
 * 操作记录业务模型
 */
namespace Model.Common.OperationRecordDTO
{
    /// <summary>
    /// 列表
    /// </summary>
    [MapFrom(typeof(Common_OperationRecord))]
    [OpenApiMainTag("List")]
    public class List : Common_OperationRecord
    {

    }

    /// <summary>
    /// 详情
    /// </summary>
    [MapFrom(typeof(Common_OperationRecord))]
    [OpenApiMainTag("Detail")]
    public class Detail : Common_OperationRecord
    {
        /// <summary>
        /// 用户详情
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        [Description("用户详情")]
        public System.UserDTO.Detail _User { get; set; }

        /// <summary>
        /// 角色详情
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        [Description("角色详情")]
        public System.RoleDTO.Detail _Role { get; set; }
    }
}
