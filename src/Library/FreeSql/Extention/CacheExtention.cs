﻿using FreeSql.Internal.Model;
using System;
using System.Collections.Generic;

namespace Microservice.Library.FreeSql.Extention
{
    /// <summary>
    /// 缓存
    /// </summary>
    internal static class CacheExtention
    {
        /// <summary>
        /// 模型对应的表字段信息
        /// </summary>
        public static Dictionary<Type, TableInfo> TableInfoDic = new Dictionary<Type, TableInfo>();

        ///// <summary>
        ///// 销毁
        ///// </summary>
        //public static void Dispose()
        //{
        //    TableInfoDic = new Dictionary<Type, TableInfo>();
        //}
    }
}
