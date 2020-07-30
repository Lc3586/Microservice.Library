using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Library.DataMapping.Application
{
    /// <summary>
    /// 成员映射
    /// </summary>
    /// <remarks>必须设置为静态属性或字段</remarks>
    public class MemberMapOptions<TSource, TDestination>
    {
        /// <summary>
        /// 映射选项
        /// </summary>
        public Dictionary<string, Action<IMemberConfigurationExpression<TSource, TDestination, object>>> Options { get; set; } = new Dictionary<string, Action<IMemberConfigurationExpression<TSource, TDestination, object>>>();

        /// <summary>
        /// 添加成员配置表达式
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="option">选项</param>
        /// <returns></returns>
        public MemberMapOptions<TSource, TDestination> Add(string name, Action<IMemberConfigurationExpression<TSource, TDestination, object>> option)
        {
            Options.Add(name, option);
            return this;
        }

        /// <summary>
        /// 添加成员映射表达式
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="option">选项</param>
        /// <returns></returns>
        public MemberMapOptions<TSource, TDestination> Add<TSourceMember>(string name, Expression<Func<TSource, TSourceMember>> option)
        {
            Options.Add(name, o => o.MapFrom(option));
            return this;
        }
    }
}
