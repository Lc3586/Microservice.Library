using Entity.System;
using Library.DataMapping.Annotations;
using Library.DataMapping.Application;
using Library.OpenApi.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;

/* 
 * 系统日志模型
 */
namespace Model.System.LogDTO
{
    /// <summary>
    /// 列表
    /// </summary>
    [MapFrom(typeof(System_Log))]
    [OpenApiMainTag("List")]
    public class List : System_Log
    {

    }

    /// <summary>
    /// 详情
    /// </summary>
    [MapFrom(typeof(System_Log))]
    [OpenApiMainTag("Detail")]
    public class Detail : System_Log
    {

    }

    /// <summary>
    /// 新增
    /// </summary>
    [MapTo(typeof(System_Log))]
    [OpenApiMainTag("Create")]
    public class Create : System_Log
    {

    }
}
