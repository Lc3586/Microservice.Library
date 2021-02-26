using Net5TC.Entity;
using Microservice.Library.DataMapping.Annotations;
using Microservice.Library.DataMapping.Application;
using Microservice.Library.OpenApi.Annotations;
using System;

/// <summary>
/// 示例实体类业务模型（数据库）
/// </summary>
namespace Net5TC.DTO.DBDTO
{
    /// <summary>
    /// 列表
    /// </summary>
    [MapFrom(typeof(Example_DB))]//映射类型配置
    [OpenApiMainTag("List")]//接口框架主标签，基类中成员附属标签不包含此标签内容的将会被筛选掉
    public class List : Example_DB
    {
        /// <summary>
        /// 值1
        /// </summary>
        public string Value1_ { get; set; }

        /// <summary>
        /// 来源成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<Example_DB, List> FromMemberMapOptions =
            new MemberMapOptions<Example_DB, List>().Add(nameof(Value1_), o => o.Value1?.ToString());
    }

    /// <summary>
    /// 详情
    /// </summary>
    [MapFrom(typeof(Example_DB))]
    [OpenApiMainTag("Detail")]
    public class Detail : Example_DB
    {
        /// <summary>
        /// 值1
        /// </summary>
        public string Value1_ { get; set; }

        /// <summary>
        /// 来源成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<Example_DB, Detail> FromMemberMapOptions =
            new MemberMapOptions<Example_DB, Detail>().Add(nameof(Value1_), o => o.Value1?.ToString());
    }

    /// <summary>
    /// 新增
    /// </summary>
    [MapTo(typeof(Example_DB))]
    [OpenApiMainTag("Create")]
    public class Create : Example_DB
    {
        /// <summary>
        /// 值1
        /// </summary>
        public string Value1_ { get; set; }

        /// <summary>
        /// 当前成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<Create, Example_DB> ToMemberMapOptions =
            new MemberMapOptions<Create, Example_DB>().Add(nameof(Value1), o => string.IsNullOrEmpty(o.Value1_)
                                                                                ? null
                                                                                : (long?)Convert.ToInt64(o.Value1_));
    }

    /// <summary>
    /// 编辑
    /// </summary>
    [MapFrom(typeof(Example_DB))]
    [MapTo(typeof(Example_DB))]
    [OpenApiMainTag("Edit")]
    public class Edit : Example_DB
    {
        /// <summary>
        /// 值1
        /// </summary>
        public string Value1_ { get; set; }

        /// <summary>
        /// 来源成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<Example_DB, Edit> FromMemberMapOptions =
            new MemberMapOptions<Example_DB, Edit>().Add(nameof(Value1_), o => o.Value1?.ToString());

        /// <summary>
        /// 当前成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<Edit, Example_DB> ToMemberMapOptions =
            new MemberMapOptions<Edit, Example_DB>().Add(nameof(Value1), o => string.IsNullOrEmpty(o.Value1_)
                                                                                ? null
                                                                                : (long?)Convert.ToInt64(o.Value1_));
    }
}
