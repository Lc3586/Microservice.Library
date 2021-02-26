using Entity.Example;
using Microservice.Library.DataMapping.Annotations;
using Microservice.Library.DataMapping.Application;
using Microservice.Library.OpenApi.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

/*
 * 示例实体类业务模型（ES服务）
*/
namespace Model.Example.ESDTO
{
    /// <summary>
    /// 列表
    /// </summary>
    [MapFrom(typeof(Sample_ES))]//映射类型配置
    [OpenApiMainTag("List")]//接口框架主标签，基类中成员附属标签不包含此标签内容的将会被筛选掉
    public class List : Sample_ES
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
        public static MemberMapOptions<Sample_ES, Detail> FromMemberMapOptions =
            new MemberMapOptions<Sample_ES, Detail>().Add(nameof(Id_), o => o.Id.ToString());
    }

    /// <summary>
    /// 详情
    /// </summary>
    [MapFrom(typeof(Sample_ES))]
    [OpenApiMainTag("Detail")]
    public class Detail : Sample_ES
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
        public static MemberMapOptions<Sample_ES, Detail> FromMemberMapOptions =
            new MemberMapOptions<Sample_ES, Detail>().Add(nameof(Id_), o => o.Id.ToString());
    }

    /// <summary>
    /// 新增
    /// </summary>
    [MapTo(typeof(Sample_ES))]
    [OpenApiMainTag("Create")]
    public class Create : Sample_ES
    {

    }

    /// <summary>
    /// 编辑
    /// </summary>
    [MapFrom(typeof(Sample_ES))]
    [MapTo(typeof(Sample_ES))]
    [OpenApiMainTag("Edit")]
    public class Edit : Sample_ES
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
        public static MemberMapOptions<Sample_ES, Detail> FromMemberMapOptions =
            new MemberMapOptions<Sample_ES, Detail>().Add(nameof(Id_), o => o.Id.ToString());

        /// <summary>
        /// 当前成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<Edit, Sample_ES> ToMemberMapOptions =
            new MemberMapOptions<Edit, Sample_ES>().Add(nameof(Id), o => Convert.ToInt64(o.Id_));
    }
}
