using Library.FreeSql.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.FreeSql.Gen
{
    public interface IFreeSqlProvider
    {
        /// <summary>
        /// 获取ORM
        /// </summary>
        /// <returns></returns>
        IFreeSql GetFreeSql();

        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        /// <returns></returns>
        BaseDbContext GetDbContext();
    }

    public class FreeSqlError : InvalidOperationException
    {
        public FreeSqlError(string title, string message = null)
            : base($"{title} : {message}")
        {

        }
    }
}
