using FreeSql;
using Library.FreeSql.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.FreeSql.Gen
{
    /// <summary>
    /// 多库构造器
    /// </summary>
    /// <typeparam name="TKey">库标识类型</typeparam>
    public interface IFreeSqlMultipleProvider<TKey>
    {
        /// <summary>
        /// 库是否存在
        /// </summary>
        /// <param name="key">库标识</param>
        bool Exists(TKey key);

        /// <summary>
        /// 库是否存在并已注册
        /// </summary>
        /// <param name="key">库标识</param>
        /// <returns></returns>
        bool ExistsAndRegistered(TKey key);

        /// <summary>
        /// 获取ORM
        /// </summary>
        /// <param name="key">库标识</param>
        /// <returns></returns>
        IFreeSql GetFreeSql(TKey key);

        /// <summary>
        /// 获取所有库的ORM
        /// </summary>
        /// <returns></returns>
        List<IFreeSql> GetAllFreeSql();

        /// <summary>
        /// 获取所有库的ORM
        /// 包括库标识
        /// </summary>
        /// <returns></returns>
        Dictionary<TKey, IFreeSql> GetAllFreeSqlWithKey();

        /// <summary>
        /// 同步结构
        /// </summary>
        /// <param name="key">库标识</param>
        void SyncStructure(TKey key);

        /// <summary>
        /// 同步所有库的结构
        /// </summary>
        void SyncAllStructure();

        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        /// <param name="key">库标识</param>
        /// <returns></returns>
        BaseDbContext GetDbContext(TKey key);

        /// <summary>
        /// 获取所有库的数据库上下文
        /// </summary>
        /// <param name="name">库名称</param>
        /// <returns></returns>
        List<BaseDbContext> GetAllDbContext();

        /// <summary>
        /// 获取所有库的数据库上下文
        /// 包括库标识
        /// </summary>
        /// <returns></returns>
        Dictionary<TKey, BaseDbContext> GetAllDbContextWithKey();
    }
}
