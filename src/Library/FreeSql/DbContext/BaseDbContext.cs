using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FreeSql;
using JetBrains.Annotations;
using Microservice.Library.FreeSql.Application;
using Microservice.Library.FreeSql.Gen;

namespace Microservice.Library.FreeSql.Repository
{
    /// <summary>
    /// DbContext容器
    /// </summary>
    public class BaseDbContext : DbContext
    {
        #region 构造函数

        public BaseDbContext(IFreeSql fsql, [NotNull]FreeSqlDbContextOptions freeSqlDbContextOptions)
        {
            _fsql = fsql;
            _freeSqlDbContextOptions = freeSqlDbContextOptions;
        }

        #endregion

        #region 外部接口

        /// <summary>
        /// 运行事务
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public (bool Success, Exception ex) RunTransaction(Action handler)
        {
            using (var tran = UnitOfWork.GetOrBeginTransaction(true))
            {
                try
                {
                    handler.Invoke();
                    tran.Commit();
                    return (true, null);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return (true, ex);
                }
            }
        }

        #endregion

        #region 私有成员

        private IFreeSql _fsql { get; set; }
        private FreeSqlDbContextOptions _freeSqlDbContextOptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseFreeSql(_fsql);
            var options = new DbContextOptions();
            if (_freeSqlDbContextOptions.EnableAddOrUpdateNavigateList.HasValue)
                options.EnableAddOrUpdateNavigateList = _freeSqlDbContextOptions.EnableAddOrUpdateNavigateList.Value;
            if (_freeSqlDbContextOptions.OnEntityChange == null)
                options.OnEntityChange = _freeSqlDbContextOptions.OnEntityChange;
            builder.UseOptions(options);
        }

        #endregion

    }
}
