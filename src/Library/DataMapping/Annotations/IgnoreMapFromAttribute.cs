﻿using System;

namespace Microservice.Library.DataMapping.Annotations
{
    /// <summary>
    /// 忽略映射来源配置
    /// </summary>
    /// <remarks>不会注册配置到 <see cref="AutoMapper.MapperConfiguration"/>中</remarks>
    public class IgnoreMapFromAttribute : Attribute
    {

    }
}
