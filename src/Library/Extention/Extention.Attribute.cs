using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Library.Extention
{
    public static partial class Extention
    {
        /// <summary>
        /// 是否为json不输出的成员
        /// </summary>
        /// <param name="element">目标成员</param>
        /// <returns></returns>
        public static bool IsJsonIgnore(this MemberInfo element)
        {
            var JI = element.GetCustomAttribute(typeof(JsonIgnoreAttribute));
            return JI != null;
        }       

        /// <summary>
        /// 获取成员的UI显示名称
        /// </summary>
        /// <param name="element">目标成员</param>
        /// <returns></returns>
        public static string GetDisplayName(this MemberInfo element)
        {
            var DA = element.GetCustomAttribute(typeof(DisplayAttribute));
            return DA == null ? string.Empty : ((DisplayAttribute)DA).Name;
        }
    }
}
