using Entity.Example;
using Library.DataMapping.Annotations;
using Library.OpenApi.Annotations;

/// <summary>
/// 示例实体类业务模型（数据库）
/// </summary>
namespace Model.Example.DB_DDTO
{
    /// <summary>
    /// 列表
    /// </summary>
    [MapFrom(typeof(Example_DB_D))]//映射类型配置
    [OpenApiMainTag("List")]//接口框架主标签，基类中成员附属标签不包含此标签内容的将会被筛选掉
    public class List : Example_DB_D
    {

    }

    /// <summary>
    /// 详情
    /// </summary>
    [MapFrom(typeof(Example_DB_D))]
    [OpenApiMainTag("Detail")]
    public class Detail : Example_DB_D
    {

    }

    /// <summary>
    /// 新增
    /// </summary>
    [MapTo(typeof(Example_DB_D))]
    [OpenApiMainTag("Create")]
    public class Create : Example_DB_D
    {

    }

    /// <summary>
    /// 编辑
    /// </summary>
    [MapFrom(typeof(Example_DB_D))]
    [MapTo(typeof(Example_DB_D))]
    [OpenApiMainTag("Edit")]
    public class Edit : Example_DB_D
    {

    }
}
