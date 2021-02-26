using Entity.System;
using Microservice.Library.DataMapping.Annotations;
using Microservice.Library.OpenApi.Annotations;

/* 
 * 资源业务模型
 */
namespace Model.System.ResourcesDTO
{
    /// <summary>
    /// 列表
    /// </summary>
    [MapFrom(typeof(System_Resources))]
    [OpenApiMainTag("List")]
    public class List : System_Resources
    {

    }

    /// <summary>
    /// 详情
    /// </summary>
    [MapFrom(typeof(System_Resources))]
    [OpenApiMainTag("Detail")]
    public class Detail : System_Resources
    {

    }

    /// <summary>
    /// 授权信息
    /// </summary>
    [MapFrom(typeof(System_Resources))]
    [OpenApiMainTag("Authorities")]
    public class Authorities : System_Resources
    {

    }

    /// <summary>
    /// 新增
    /// </summary>
    [MapTo(typeof(System_Resources))]
    [OpenApiMainTag("Create")]
    public class Create : System_Resources
    {

    }

    /// <summary>
    /// 编辑
    /// </summary>
    [MapFrom(typeof(System_Resources))]
    [MapTo(typeof(System_Resources))]
    [OpenApiMainTag("Edit")]
    public class Edit : System_Resources
    {

    }
}
