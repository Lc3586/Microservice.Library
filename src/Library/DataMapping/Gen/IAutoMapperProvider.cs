using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.DataMapping.Gen
{
    public interface IAutoMapperProvider
    {
        /// <summary>
        /// 获取映射器
        /// </summary>
        /// <returns></returns>
        IMapper GetMapper();
    }

    /// <summary>
    /// 异常
    /// </summary>
    /// <returns></returns>
    public class AutoMapperError : InvalidOperationException
    {
        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">内容</param>
        public AutoMapperError(string title, string message = null)
            : base($"{title} : {message}")
        {

        }
    }
}
