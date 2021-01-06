﻿using Entity.Example;
using Entity.System;
using Library.DataMapping.Annotations;
using Library.DataMapping.Application;
using Library.OpenApi.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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
    /// 基础信息
    /// </summary>
    [MapFrom(typeof(System_Resources))]
    [OpenApiMainTag("Base")]
    public class Base : System_Resources
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
