﻿using Microservice.Library.Extension.Helper;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservice.Library.DataAccess
{
    public static class ShardingHelper
    {
        /// <summary>
        /// 映射物理表
        /// </summary>
        /// <param name="absTable">抽象表类型</param>
        /// <param name="targetTableName">目标物理表名</param>
        /// <returns></returns>
        public static Type MapTable(Type absTable, string targetTableName)
        {
            var config = TypeBuilderHelper.GetConfig(absTable);

            //实体必须放到Entity层中,不然会出现莫名调试BUG,原因未知
            config.AssemblyName = "Entity";
            config.Attributes.RemoveAll(x => x.Attribute == typeof(TableAttribute));
            config.FullName = $"Entity.{targetTableName}";

            return TypeBuilderHelper.BuildType(config);
        }
    }
}
