using Entity.Public;
using Library.DataMapping.Annotations;
using Library.OpenApi.Annotations;
using System.Collections.Generic;
using System.ComponentModel;

/* 
 * 会员业务模型
 */
namespace Model.Public.MemberDTO
{
    /// <summary>
    /// 列表
    /// </summary>
    [MapFrom(typeof(Public_Member))]
    [OpenApiMainTag("List")]
    public class List : Public_Member
    {

    }

    /// <summary>
    /// 详情
    /// </summary>
    [MapFrom(typeof(Public_Member))]
    [OpenApiMainTag("Detail")]
    public class Detail : Public_Member
    {

    }

    /// <summary>
    /// 新增
    /// </summary>
    [MapTo(typeof(Public_Member))]
    [OpenApiMainTag("Create")]
    public class Create : Public_Member
    {

    }

    /// <summary>
    /// 编辑
    /// </summary>
    [MapFrom(typeof(Public_Member))]
    [MapTo(typeof(Public_Member))]
    [OpenApiMainTag("Edit")]
    public class Edit : Public_Member
    {

    }

    /// <summary>
    /// 授权信息
    /// </summary>
    [MapFrom(typeof(Public_Member))]
    [OpenApiMainTag("Authorities")]
    public class Authorities : Public_Member
    {
        /// <summary>
        /// 授权给此会员的角色
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        [Description("授权给此会员的角色")]
        public List<System.RoleDTO.Authorities> _Roles { get; set; }
    }
}
