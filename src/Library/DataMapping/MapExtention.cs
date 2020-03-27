using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.DataMapping
{
    public static class MapExtention
    {
        /// <summary>
        /// 获取主标签名称
        /// <para>默认为类型名称</para>
        /// </summary>
        /// <param name="type">目标对象</param>
        /// <remarks>LCTR 2020-03-10</remarks>
        /// <returns></returns>
        public static List<(string Name, Action<IMemberConfigurationExpression> Option)> GetForMembersOptions(this Type type, bool isFrom)
        {
            var prop = type.GetProperty(isFrom ? "FromForMembersOptions" : "ToForMembersOptions");
            return prop?.GetValue(null, null) as List<(string, Action<IMemberConfigurationExpression>)>;
        }
    }
}
