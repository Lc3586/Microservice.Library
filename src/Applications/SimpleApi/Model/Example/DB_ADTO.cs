using Entity.Example;
using Library.DataMapping.Annotations;
using Library.OpenApi.Annotations;
using System.Collections.Generic;

/// <summary>
/// 示例实体类业务模型（数据库）
/// </summary>
namespace Model.Example.DB_ADTO
{
    /// <summary>
    /// 列表
    /// </summary>
    [MapFrom(typeof(Example_DB_A))]//映射类型配置
    [OpenApiMainTag("List")]//接口框架主标签，基类中成员附属标签不包含此标签内容的将会被筛选掉
    public class List : Example_DB_A
    {
        /// <summary>
        /// 所属B
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public DB_BDTO.List DB_B { get; set; }

        /// <summary>
        /// 相关C集合
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<DB_CDTO.List> DB_Cs { get; set; }

        /// <summary>
        /// 相关D集合
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<DB_DDTO.List> DB_Ds { get; set; }
    }

    /// <summary>
    /// 详情
    /// </summary>
    [MapFrom(typeof(Example_DB_A))]
    [OpenApiMainTag("Detail")]
    public class Detail : Example_DB_A
    {
        /// <summary>
        /// 所属B
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public DB_BDTO.Detail DB_B { get; set; }

        /// <summary>
        /// 相关C集合
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<DB_CDTO.Detail> DB_Cs { get; set; }

        /// <summary>
        /// 相关D集合
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<DB_DDTO.Detail> DB_Ds { get; set; }
    }

    /// <summary>
    /// 新增
    /// </summary>
    [MapTo(typeof(Example_DB_A))]
    [OpenApiMainTag("Create")]
    public class Create : Example_DB_A
    {
        /// <summary>
        /// 相关CId集合
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<string> DB_CIDs { get; set; }

        /// <summary>
        /// 相关D集合
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<DB_DDTO.Create> DB_Ds { get; set; }
    }

    /// <summary>
    /// 编辑
    /// </summary>
    [MapFrom(typeof(Example_DB_A))]
    [MapTo(typeof(Example_DB_A))]
    [OpenApiMainTag("Edit")]
    public class Edit : Example_DB_A
    {
        /// <summary>
        /// 所属B
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public DB_BDTO.Edit DB_B { get; set; }

        /// <summary>
        /// 相关C集合
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<string> DB_CIDs { get; set; }

        /// <summary>
        /// 相关D集合
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<DB_DDTO.Edit> DB_Ds { get; set; }
    }
}
