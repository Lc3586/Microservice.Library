using AutoMapper;
using AutoMapper.Internal;
using Microservice.Library.DataMapping.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microservice.Library.DataMapping.Extention
{
    /// <summary>
    /// 数据映射拓展方法
    /// </summary>
    public static class MapExtention
    {
        /// <summary>
        /// 配置数据映射来源
        /// </summary>
        /// <param name="configure"></param>
        /// <param name="type"></param>
        public static void CreateMapFrom(this IMapperConfigurationExpression configure, Type type)
        {
            IMapAttribute attribute = type.GetCustomAttribute<MapFromAttribute>();
            if (attribute == null)
                return;

            var ignore = type.GetCustomAttribute<IgnoreMapFromAttribute>();
            if (ignore != null)
                return;

            configure.CreateMap(type, attribute.Types, attribute.FromOrTo);
        }

        /// <summary>
        /// 配置数据映射来源
        /// </summary>
        /// <param name="configure"></param>
        /// <param name="type"></param>
        public static void CreateMapTo(this IMapperConfigurationExpression configure, Type type)
        {
            IMapAttribute attribute = type.GetCustomAttribute<MapToAttribute>();
            if (attribute == null)
                return;

            var ignore = type.GetCustomAttribute<IgnoreMapToAttribute>();
            if (ignore != null)
                return;

            configure.CreateMap(type, attribute.Types, attribute.FromOrTo);
        }

        /// <summary>
        /// 配置数据映射
        /// </summary>
        /// <param name="configure"></param>
        /// <param name="soureType"></param>
        /// <param name="destinationTypes"></param>
        /// <param name="fromOrTo"></param>
        public static void CreateMap(this IMapperConfigurationExpression configure, Type soureType, List<Type> destinationTypes, bool fromOrTo)
        {
            foreach (var destinationType in destinationTypes)
            {
                var map = fromOrTo ? configure.CreateMap(destinationType, soureType) : configure.CreateMap(soureType, destinationType);

                var options = soureType.GetMemberMapOptions(fromOrTo);

                if(options!=null)
                    foreach (var item in options)
                    {
                        map.ForMember(item.Key, item.Value);
                    }
            }
        }

        /// <summary>
        /// 获取成员映射选项
        /// <para>默认为类型名称</para>
        /// </summary>
        /// <param name="type">目标对象</param>
        /// <param name="fromOrTo"></param>
        /// <returns></returns>
        public static Dictionary<string, Action<IMemberConfigurationExpression>> GetMemberMapOptions(this Type type, bool fromOrTo)
        {
            //if (!type.IsAssignableFrom(typeof(IMap)))
            //    return null;

            var memberName = fromOrTo ? "FromMemberMapOptions" : "ToMemberMapOptions";

            var member = type.GetMember(memberName)?.FirstOrDefault(m => m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property);

            if (member == null)
                return null;

            var options = member.GetMemberValue(type);

            if (options == null)
                return null;

            return options.GetType().GetMember("Options")[0].GetMemberValue(options) as Dictionary<string, Action<IMemberConfigurationExpression>>;
        }
    }
}
