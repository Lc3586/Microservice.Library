using AutoMapper;
using Library.DataMapping;
using Library.Extention;
using Library.OpenApi.Annotations;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;
using ExampleEntity = Integrate_Entity.Example.Example;//当类和命名空间重名时使用别名（任何情况下都要避免出现重名）

namespace Integrate_Model.Example
{
    /// <summary>
    /// 示例业务模型类
    /// </summary>
    public class ExampleDTO
    {
        /// <summary>
        /// 列表
        /// </summary>
        [MapFrom(typeof(ExampleEntity))]//映射类型配置
        [OpenApiMainTag("List")]//接口框架主标签，基类中成员附属标签不包含此标签内容的将会被筛选掉
        public class List : ExampleEntity
        {
            /// <summary>
            /// Id
            /// </summary>
            [OpenApiSchema(OpenApiSchemaType._integer, OpenApiSchemaFormat.integer_int64)]//接口框架属性
            public new string Id { get; set; }
        }

        /// <summary>
        /// 详情
        /// </summary>
        [MapFrom(typeof(ExampleEntity), true)]
        [OpenApiMainTag("Detail")]
        public class Detail : ExampleEntity
        {
            /// <summary>
            /// Id
            /// </summary>
            [OpenApiSchema(OpenApiSchemaType._integer, OpenApiSchemaFormat.integer_int64)]//接口框架属性
            public new string Id { get; set; }

            /// <summary>
            /// 来源成员映射选项
            /// </summary>
            [OpenApiIgnore]
            public static List<(string, Action<IMemberConfigurationExpression>)> FromForMembersOptions
            {
                get
                {
                    return new List<(string, Action<IMemberConfigurationExpression>)>()
                       {
                            ("Id",o => o.MapFrom(s=>((ExampleEntity)s).Id.ToString()))
                       };
                }
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        [MapTo(typeof(ExampleEntity))]
        [OpenApiMainTag("Create")]
        public class Create : ExampleEntity
        {

        }

        /// <summary>
        /// 编辑
        /// </summary>
        [MapFrom(typeof(ExampleEntity), true)]
        [MapTo(typeof(ExampleEntity), true)]
        [OpenApiMainTag("Edit")]
        public class Edit : ExampleEntity
        {
            /// <summary>
            /// Id
            /// </summary>
            [OpenApiSchema(OpenApiSchemaType._integer, OpenApiSchemaFormat.integer_int64)]//接口框架属性
            [Required(ErrorMessage = "Id不可为空")]//非空验证
            public new string Id { get; set; }

            /// <summary>
            /// 当前成员映射选项
            /// </summary>
            [OpenApiIgnore]
            public static List<(string, Action<IMemberConfigurationExpression>)> ToForMembersOptions
            {
                get
                {
                    return new List<(string, Action<IMemberConfigurationExpression>)>()
                        {
                            ("Id",o => o.MapFrom(s=>Convert.ToInt64(((Edit)s).Id)))
                        };
                }
            }

            /// <summary>
            /// 来源成员映射选项
            /// </summary>
            [OpenApiIgnore]
            public static List<(string, Action<IMemberConfigurationExpression>)> FromForMembersOptions
            {
                get
                {
                    return new List<(string, Action<IMemberConfigurationExpression>)>()
                       {
                            ("Id",o => o.MapFrom(s=>((ExampleEntity)s).Id.ToString()))
                       };
                }
            }
        }
    }
}
