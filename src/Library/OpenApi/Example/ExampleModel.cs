using Library.OpenApi.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Library.OpenApi.ExampleModel
{
    /// <summary>
    /// A信息
    /// </summary>
    [OpenApiMainTag("Info")]
    public class AInfo : Example_A
    {
        /// <summary>
        /// 所属B
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public BInfo B { get; set; }

        /// <summary>
        /// 相关C集合
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<CInfo> Cs { get; set; }

        /// <summary>
        /// 相关D集合
        /// </summary>
        [OpenApiSchema(OpenApiSchemaType.model)]
        public List<DInfo> Ds { get; set; }
    }

    /// <summary>
    /// B信息
    /// </summary>
    [OpenApiMainTag("Info")]
    public class BInfo : Example_B
    {

    }

    /// <summary>
    /// C信息
    /// </summary>
    [OpenApiMainTag("Info")]
    public class CInfo : Example_C
    {

    }

    /// <summary>
    /// D信息
    /// </summary>
    [OpenApiMainTag("Info")]
    public class DInfo : Example_D
    {

    }

    /// <summary>
    /// A
    /// </summary>
    public class Example_A
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 父AId
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 所属B
        /// </summary>
        public string BId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [OpenApiSubTag("Info", "Create", "Edit", "Detail")]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [OpenApiSubTag("Create", "Edit", "Detail")]
        [Description("内容")]
        public string Content { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        [OpenApiSubTag("Info", "Detail")]
        [Description("创建者")]
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [OpenApiSubTag("Info", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近修改时间
        /// </summary>
        [OpenApiSubTag("Info", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [Description("最近修改时间")]
        public DateTime ModifyTime { get; set; }
    }

    /// <summary>
    /// B
    /// </summary>
    public class Example_B
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [OpenApiSubTag("Info", "Create", "Edit", "Detail")]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        [OpenApiSubTag("Info", "Detail")]
        [Description("创建者")]
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [OpenApiSubTag("Info", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近修改时间
        /// </summary>
        [OpenApiSubTag("Info", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, "", OpenApiSchemaFormat.string_datetime)]
        [Description("最近修改时间")]
        public DateTime ModifyTime { get; set; }
    }

    /// <summary>
    /// C
    /// </summary>
    public class Example_C
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [OpenApiSubTag("Info", "Create", "Edit", "Detail")]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        [OpenApiSubTag("Info", "Detail")]
        [Description("创建者")]
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [OpenApiSubTag("Info", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近修改时间
        /// </summary>
        [OpenApiSubTag("Info", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, "", OpenApiSchemaFormat.string_datetime)]
        [Description("最近修改时间")]
        public DateTime ModifyTime { get; set; }
    }

    /// <summary>
    /// D
    /// </summary>
    public class Example_D
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 所属A
        /// </summary>
        public string AId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [OpenApiSubTag("Info", "Create", "Edit", "Detail")]
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        [OpenApiSubTag("Info", "Detail")]
        [Description("创建者")]
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [OpenApiSubTag("Info", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近修改时间
        /// </summary>
        [OpenApiSubTag("Info", "Detail")]
        [OpenApiSchema(OpenApiSchemaType.@string, OpenApiSchemaFormat.string_datetime)]
        [Description("最近修改时间")]
        public DateTime ModifyTime { get; set; }
    }
}
