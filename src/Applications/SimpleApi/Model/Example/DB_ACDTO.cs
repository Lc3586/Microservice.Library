using Entity.Example;
using Library.DataMapping.Annotations;
using Library.OpenApi.Annotations;

/// <summary>
/// 示例实体类业务模型（数据库）
/// </summary>
namespace Model.Example.DB_ACDTO
{
    /// <summary>
    /// 列表
    /// </summary>
    [MapFrom(typeof(Example_DB_B))]//映射类型配置
    [OpenApiMainTag("List")]//接口框架主标签，基类中成员附属标签不包含此标签内容的将会被筛选掉
    public class List : Example_DB_B
    {

    }

    /// <summary>
    /// 详情
    /// </summary>
    [MapFrom(typeof(Example_DB_B))]
    [OpenApiMainTag("Detail")]
    public class Detail : Example_DB_B
    {

    }
}
