using FreeSql;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.FreeSql.Extention
{
    /// <summary>
    /// Repository扩展类
    /// </summary>
    public static class RepositoryExtension
    {
        /// <summary>
        /// 获取数据并验证非空
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="repository"></param>
        /// <param name="id"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static TSource GetAndCheckNull<TSource, TKey>(this IBaseRepository<TSource, TKey> repository, TKey id, string error = "数据不存在或已失效") where TSource : class
        {
            var data = repository.Get(id);
            if (data == null)
                throw new MessageException(error);
            return data;
        }
    }
}
