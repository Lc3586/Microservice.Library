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

    public class FreeSqlError : InvalidOperationException
    {
        public FreeSqlError(string title, string message = null)
            : base($"{title} : {message}")
        {

        }
    }
}
