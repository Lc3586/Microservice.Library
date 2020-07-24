using FreeSql;
using Library.FreeSql.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.FreeSql.Gen
{
    /// <summary>
    /// 单库构造器
    /// </summary>
    public interface IFreeSqlProvider
    {
        /// <summary>
        /// 获取ORM构建器
        /// </summary>
        /// <returns></returns>
        FreeSqlBuilder GetFreeSqlBuilder();

        /// <summary>
        /// 获取ORM
        /// </summary>
        /// <returns></returns>
        IFreeSql GetFreeSql();

        /// <summary>
        /// 同步结构
        /// </summary>
        void SyncStructure();

        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        /// <returns></returns>
        BaseDbContext GetDbContext();
    }
}
