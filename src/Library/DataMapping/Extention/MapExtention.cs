using AutoMapper;
using AutoMapper.Internal;
using Library.DataMapping.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.DataMapping.Extention
{
    /// <summary>
    /// 拓展方法
    /// </summary>
    public static class MapExtention
    {
        /// <summary>
        /// 获取成员映射选项
        /// <para>默认为类型名称</para>
        /// </summary>
        /// <param name="type">目标对象</param>
        /// <param name="isFrom">是否为映射来源</param>
        /// <remarks>LCTR 2020-03-10</remarks>
        /// <returns></returns>
        public static Dictionary<string, Action<IMemberConfigurationExpression>> GetMemberMapOptions(this Type type, bool isFrom)
        {
            var name = isFrom ? "FromMemberMapOptions" : "ToMemberMapOptions";
            var member = type.GetMember(name)?.FirstOrDefault(m => m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property);

            if (member == null)
                return null;

            var options = member.GetMemberValue(type);

            return options.GetType().GetMember("Options")[0].GetMemberValue(options) as Dictionary<string, Action<IMemberConfigurationExpression>>;
        }

        /// <summary>
        /// 配置Map
        /// </summary>
        /// <param name="configure"></param>
        /// <param name="type"></param>
        public static void CreateMap<T>(this IMapperConfigurationExpression configure, Type type) where T : Attribute
        {
            var attribute = type.GetCustomAttribute<T>() as IMapAttribute;
            if (attribute == null)
                return;

            var map = attribute.IsFrom ? configure.CreateMap(attribute.Type, type) : configure.CreateMap(type, attribute.Type);
            if (!attribute.EnableMemberMap)
                return;

            var options = type.GetMemberMapOptions(attribute.IsFrom);

            if (options != null)
            {
                foreach (var option in type.GetMemberMapOptions(attribute.IsFrom))
                {
                    map.ForMember(option.Key, option.Value);
                }
            }
        }
    }
}
