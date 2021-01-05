using Entity.Example;
using Library.DataMapping.Annotations;
using Library.DataMapping.Application;
using Library.OpenApi.Annotations;
using System;

/*
 * 示例实体类业务模型（数据库）
*/
namespace Model.Example.DBDTO
{
    /// <summary>
    /// 列表
    /// </summary>
    [MapFrom(typeof(Sample_DB))]//映射类型配置
    [OpenApiMainTag("List")]//接口框架主标签，基类中成员附属标签不包含此标签内容的将会被筛选掉
    public class List : Sample_DB
    {
        /// <summary>
        /// 值1
        /// </summary>
        public string Value1_ { get; set; }

        /// <summary>
        /// 来源成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<Sample_DB, List> FromMemberMapOptions =
            new MemberMapOptions<Sample_DB, List>().Add(nameof(Value1_), o => o.Value1?.ToString());
    }

    /// <summary>
    /// 详情
    /// </summary>
    [MapFrom(typeof(Sample_DB))]
    [OpenApiMainTag("Detail")]
    public class Detail : Sample_DB
    {
        /// <summary>
        /// 值1
        /// </summary>
        public string Value1_ { get; set; }

        /// <summary>
        /// 来源成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<Sample_DB, Detail> FromMemberMapOptions =
            new MemberMapOptions<Sample_DB, Detail>().Add(nameof(Value1_), o => o.Value1?.ToString());
    }

    /// <summary>
    /// 新增
    /// </summary>
    [MapTo(typeof(Sample_DB))]
    [OpenApiMainTag("Create")]
    public class Create : Sample_DB
    {
        /// <summary>
        /// 值1
        /// </summary>
        public string Value1_ { get; set; }

        /// <summary>
        /// 当前成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<Create, Sample_DB> ToMemberMapOptions =
            new MemberMapOptions<Create, Sample_DB>().Add(nameof(Value1), o => string.IsNullOrEmpty(o.Value1_)
                                                                                ? null
                                                                                : (long?)Convert.ToInt64(o.Value1_));
    }

    /// <summary>
    /// 编辑
    /// </summary>
    [MapFrom(typeof(Sample_DB))]
    [MapTo(typeof(Sample_DB))]
    [OpenApiMainTag("Edit")]
    public class Edit : Sample_DB
    {
        /// <summary>
        /// 值1
        /// </summary>
        public string Value1_ { get; set; }

        /// <summary>
        /// 来源成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<Sample_DB, Edit> FromMemberMapOptions =
            new MemberMapOptions<Sample_DB, Edit>().Add(nameof(Value1_), o => o.Value1?.ToString());

        /// <summary>
        /// 当前成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<Edit, Sample_DB> ToMemberMapOptions =
            new MemberMapOptions<Edit, Sample_DB>().Add(nameof(Value1), o => string.IsNullOrEmpty(o.Value1_)
                                                                                ? null
                                                                                : (long?)Convert.ToInt64(o.Value1_));
    }
}
