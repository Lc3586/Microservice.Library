using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Library.DataMapping.Application
{
    /// <summary>
    /// 成员映射
    /// </summary>
    /// <remarks>必须设置为静态属性或字段</remarks>
    public class MemberMapOptions<TSource, TDestination> where TSource : class where TDestination : class
    {
        /// <summary>
        /// 映射选项
        /// </summary>
        public Dictionary<string, Action<IMemberConfigurationExpression>> Options { get; set; } = new Dictionary<string, Action<IMemberConfigurationExpression>>();

        /// <summary>
        /// 添加成员配置表达式
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="option">选项</param>
        /// <returns></returns>
        public MemberMapOptions<TSource, TDestination> Add(string name, Action<IMemberConfigurationExpression> option)
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
        public MemberMapOptions<TSource, TDestination> Add<TSourceMember>(string name, Func<TSource, TSourceMember> option)
        {
            Options.Add(name, o => o.MapFrom(source => option.Invoke(source as TSource)));
            return this;
        }
    }
}
