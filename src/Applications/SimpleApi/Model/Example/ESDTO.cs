using Entity.Example;
using Library.DataMapping.Annotations;
using Library.DataMapping.Application;
using Library.OpenApi.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// 示例实体类业务模型（ES服务）
/// </summary>
namespace Model.Example.ESDTO
{
    /// <summary>
    /// 列表
    /// </summary>
    [MapFrom(typeof(Example_ES))]//映射类型配置
    [OpenApiMainTag("List")]//接口框架主标签，基类中成员附属标签不包含此标签内容的将会被筛选掉
    public class List : Example_ES
    {
        /// <summary>
        /// Id
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@string)]//接口框架属性
        public string Id_ { get; set; }

        /// <summary>
        /// 来源成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<Example_ES, Detail> FromMemberMapOptions =
            new MemberMapOptions<Example_ES, Detail>().Add(nameof(Id_), o => o.Id.ToString());
    }

    /// <summary>
    /// 详情
    /// </summary>
    [MapFrom(typeof(Example_ES))]
    [OpenApiMainTag("Detail")]
    public class Detail : Example_ES
    {
        /// <summary>
        /// Id
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@string)]//接口框架属性
        public string Id_ { get; set; }

        /// <summary>
        /// 来源成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<Example_ES, Detail> FromMemberMapOptions =
            new MemberMapOptions<Example_ES, Detail>().Add(nameof(Id_), o => o.Id.ToString());
    }

    /// <summary>
    /// 新增
    /// </summary>
    [MapTo(typeof(Example_ES))]
    [OpenApiMainTag("Create")]
    public class Create : Example_ES
    {

    }

    /// <summary>
    /// 编辑
    /// </summary>
    [MapFrom(typeof(Example_ES))]
    [MapTo(typeof(Example_ES))]
    [OpenApiMainTag("Edit")]
    public class Edit : Example_ES
    {
        /// <summary>
        /// Id
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.@string)]//接口框架属性
        [Required(ErrorMessage = "Id不可为空")]//非空验证
        public string Id_ { get; set; }

        /// <summary>
        /// 来源成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<Example_ES, Detail> FromMemberMapOptions =
            new MemberMapOptions<Example_ES, Detail>().Add(nameof(Id_), o => o.Id.ToString());

        /// <summary>
        /// 当前成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<Edit, Example_ES> ToMemberMapOptions =
            new MemberMapOptions<Edit, Example_ES>().Add(nameof(Id), o => Convert.ToInt64(o.Id_));
    }
}
