using Library.DataRepository;
using Library.Extention;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Common;
using System.Reflection;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using Npgsql;
using Library.Models;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;

namespace Library.DataRepository_PostgreSql
{
    public class DbRepository : IRepository
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="entityAssembly">数据库实体命名空间,注意,该命名空间应该包含所有需要的数据库实体</param>
        public DbRepository(string connectionString, string entityAssembly)
        {
            ConnectionString = connectionString;
            EntityAssembly = entityAssembly;
        }

        #endregion

        #region 私有成员

        protected RepositoryDbContext Db
        {
            get
            {
                if (_disposed || _db == null)
                {
                    _db = new RepositoryDbContext(ConnectionString, EntityAssembly);
                    _disposed = false;
                }

                return _db;
            }
            set
            {
                _db = value;
            }
        }
        private RepositoryDbContext _db { get; set; }
        protected bool _disposed { get; set; }
        protected DbTransaction _transaction { get; set; }
        protected static PropertyInfo GetKeyProperty(Type type)
        {
            return GetKeyPropertys(type).FirstOrDefault();
        }
        protected static List<PropertyInfo> GetKeyPropertys(Type type)
        {
            var properties = type
                .GetProperties()
                .Where(x => x.GetCustomAttributes(true).Select(o => o.GetType().FullName).Contains(typeof(KeyAttribute).FullName))
                .ToList();

            return properties;
        }
        protected string GetDbTableName(Type type)
        {
            string tableName = string.Empty;
            var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            if (tableAttribute != null)
                tableName = tableAttribute.Name;
            else
                tableName = type.Name;

            return tableName;
        }
        private void PackWork(IEnumerable<Type> entityTypes, Action work)
        {
            entityTypes.ForEach(x => Db.CheckEntityType(x));
            work();
            CommitDb();
            if (!_openedTransaction)
                Dispose();
        }
        protected bool _openedTransaction { get; set; } = false;
        protected virtual string FormatFieldName(string name)
        {
            throw new NotImplementedException("请在子类实现!");
        }
        protected virtual string FormatParamterName(string name)
        {
            return $"@{name}";
        }
        private (string sql, Dictionary<string, object> paramters) GetWhereSql(IQueryable query)
        {
            Dictionary<string, object> paramters = new Dictionary<string, object>();
            var querySql = query.ToSql();
            string theQSql = querySql.sql.Replace("\r\n", "\n").Replace("\n", " ");
            //无筛选
            if (!theQSql.Contains("WHERE"))
                return (" 1=1 ", paramters);

            string pattern1 = "^SELECT.*?FROM.*? AS (.*?) WHERE .*?$";
            string pattern2 = "^SELECT.*?FROM .*? (.*?) WHERE .*?$";
            string asTmp = string.Empty;
            if (Regex.IsMatch(theQSql, pattern1))
            {
                var match = Regex.Match(theQSql, pattern1);
                asTmp = match.Groups[1]?.ToString();
            }
            else if (Regex.IsMatch(theQSql, pattern2))
            {
                var match = Regex.Match(theQSql, pattern2);
                asTmp = match.Groups[1]?.ToString();
            }
            if (asTmp.IsNullOrEmpty())
                throw new Exception("SQL解析失败!");

            string whereSql = querySql.sql.Split(new string[] { "WHERE" }, StringSplitOptions.None)[1].Replace($"{asTmp}.", "");

            querySql.parameters.ForEach(aData =>
            {
                if (whereSql.Contains(aData.Key))
                    paramters.Add(aData.Key, aData.Value);
            });

            return (whereSql, paramters);
        }

        #endregion

        #region 事物相关

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            _openedTransaction = true;
            _transaction = Db.Database.BeginTransaction(isolationLevel).GetDbTransaction();

            Db.UseTransaction(_transaction);
        }

        public (bool Success, Exception ex) RunTransaction(Action action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            bool success = true;
            Exception resEx = null;
            try
            {
                BeginTransaction(isolationLevel);

                action();

                CommitTransaction();
            }
            catch (Exception ex)
            {
                success = false;
                resEx = ex;
                RollbackTransaction();
            }
            finally
            {
                _openedTransaction = false;
                Dispose();
            }

            return (success, resEx);
        }

        #endregion

        #region 数据库相关

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// 实体类命名空间
        /// </summary>
        private string EntityAssembly { get; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType DbType { get { return DatabaseType.PostgreSQL; } }

        /// <summary>
        /// 提交到数据库
        /// </summary>
        public void CommitDb()
        {
            Db.SaveChanges();
        }

        /// <summary>
        /// 提交事物
        /// </summary>
        public void CommitTransaction()
        {
            _transaction?.Commit();
        }

        /// <summary>
        /// 回滚事物
        /// </summary>
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
        }

        /// <summary>
        /// 获取DbContext
        /// </summary>
        /// <returns></returns>
        public DbContext GetDbContext()
        {
            return Db.GetDbContext();
        }

        /// <summary>
        /// SQL日志处理方法
        /// </summary>
        /// <value>
        /// The handle SQL log.
        /// </value>
        public Action<string> HandleSqlLog { set => EFCoreSqlLogeerProvider.HandleSqlLog = value; }

        /// <summary>
        /// 使用已存在的事物
        /// </summary>
        /// <param name="transaction">事物对象</param>
        public void UseTransaction(DbTransaction transaction)
        {
            if (_transaction != null)
                _transaction.Dispose();

            _openedTransaction = true;
            _transaction = transaction;
            Db.UseTransaction(transaction);
        }

        /// <summary>
        /// 获取事物对象
        /// </summary>
        /// <returns></returns>
        public DbTransaction GetTransaction()
        {
            return _transaction;
        }

        #endregion

        #region 增加数据

        /// <summary>
        /// 添加单条记录
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="entity">实体对象</param>
        public void Insert<T>(T entity) where T : class, new()
        {
            Insert(new List<object> { entity });
        }

        /// <summary>
        /// 添加多条记录
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="entities">实体对象集合</param>
        public void Insert<T>(List<T> entities) where T : class, new()
        {
            Insert(entities.CastToList<object>());
        }

        /// <summary>
        /// 添加多条记录
        /// </summary>
        /// <param name="entities">对象集合</param>
        public void Insert(List<object> entities)
        {
            PackWork(entities.Select(x => x.GetType()), () =>
            {
                entities.ForEach(x => Db.Entry(x).State = EntityState.Added);
            });
        }

        /// <summary>
        /// 使用Bulk批量导入,速度快
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="entities">实体集合</param>
        /// <exception cref="NotImplementedException">不支持此操作!</exception>
        public virtual void BulkInsert<T>(List<T> entities) where T : class, new()
        {
            throw new Exception("PostgreSql暂不支持此操作！");
        }

        #endregion

        #region 删除数据

        /// <summary>
        /// 删除所有记录
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        public virtual void DeleteAll<T>() where T : class, new()
        {
            Delete(GetList<T>());
        }

        /// <summary>
        /// 删除所有记录
        /// </summary>
        /// <param name="type">实体类型</param>
        public void DeleteAll(Type type)
        {
            string tableName = GetDbTableName(type);
            string sql = $"DELETE FROM {tableName}";
            ExecuteSql(sql);
        }

        /// <summary>
        /// 删除单条记录
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="entity">实体对象</param>
        public void Delete<T>(T entity) where T : class, new()
        {
            Delete(new List<object> { entity });
        }

        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="entities">实体对象集合</param>
        public void Delete<T>(List<T> entities) where T : class, new()
        {
            Delete(entities.CastToList<object>());
        }

        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        public void Delete(List<object> entities)
        {
            PackWork(entities.Select(x => x.GetType()), () =>
            {
                entities.ForEach(x => Db.Entry(x).State = EntityState.Deleted);
            });
        }

        /// <summary>
        /// 按条件删除记录
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="condition">筛选条件</param>
        public void Delete<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            var deleteList = GetIQueryable<T>().Where(condition).ToList();
            Delete(deleteList);
        }

        /// <summary>
        /// 删除单条记录
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="key">主键</param>
        public void Delete<T>(string key) where T : class, new()
        {
            Delete<T>(new List<string> { key });
        }

        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="keys">多条记录主键集合</param>
        public void Delete<T>(List<string> keys) where T : class, new()
        {
            Delete(typeof(T), keys);
        }

        /// <summary>
        /// 删除单条记录
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="key">主键</param>
        public void Delete(Type type, string key)
        {
            Delete(type, new List<string> { key });
        }

        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="keys">多条记录主键集合</param>
        /// <exception cref="Exception">该实体没有主键标识！请使用[Key]标识主键！</exception>
        public void Delete(Type type, List<string> keys)
        {
            var theProperty = GetKeyProperty(type);
            if (theProperty == null)
                throw new Exception("该实体没有主键标识！请使用[Key]标识主键！");

            List<object> deleteList = new List<object>();
            keys.ForEach(aKey =>
            {
                object newData = Activator.CreateInstance(type);
                var value = aKey.ChangeType(theProperty.PropertyType);
                theProperty.SetValue(newData, value);
                deleteList.Add(newData);
            });

            Delete(deleteList);
        }

        /// <summary>
        /// 使用SQL语句按照条件删除数据
        /// 用法:Delete_Sql"Base_User"(x=&gt;x.Id == "Admin")
        /// 注：生成的SQL类似于DELETE FROM [Base_User] WHERE [Name] = 'xxx' WHERE [Id] = 'Admin'
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="where">条件</param>
        /// <returns>
        /// 影响条数
        /// </returns>
        public int Delete_Sql<T>(Expression<Func<T, bool>> where) where T : class, new()
        {
            var iq = GetIQueryable<T>().Where(where);

            return _Delete_Sql(iq);
        }

        public int Delete_Sql(Type entityType, string where, params object[] paramters)
        {
            var iq = GetIQueryable(entityType).Where(where, paramters);

            return _Delete_Sql(iq);
        }

        public virtual int _Delete_Sql(IQueryable iq)
        {
            throw new NotImplementedException("接口未实现");
        }

        #endregion

        #region 更新数据

        /// <summary>
        /// 更新单条记录
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="entity">实体对象</param>
        public void Update<T>(T entity) where T : class, new()
        {
            Update(new List<object> { entity });
        }

        /// <summary>
        /// 更新多条记录
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="entities">实体对象集合</param>
        public void Update<T>(List<T> entities) where T : class, new()
        {
            Update(entities.CastToList<object>());
        }

        /// <summary>
        /// 更新多条记录
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        public void Update(List<object> entities)
        {
            PackWork(entities.Select(x => x.GetType()), () =>
            {
                entities.ForEach(x => Db.Entry(x).State = EntityState.Modified);
            });
        }

        /// <summary>
        /// 更新单条记录的某些属性
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="properties">属性</param>
        public void UpdateAny<T>(T entity, List<string> properties) where T : class, new()
        {
            UpdateAny(new List<object> { entity }, properties);
        }

        /// <summary>
        /// 更新多条记录的某些属性
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="entities">实体对象集合</param>
        /// <param name="properties">属性</param>
        public void UpdateAny<T>(List<T> entities, List<string> properties) where T : class, new()
        {
            UpdateAny(entities.CastToList<object>(), properties);
        }

        /// <summary>
        /// 更新多条记录的某些属性
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <param name="properties">属性</param>
        public void UpdateAny(List<object> entities, List<string> properties)
        {
            PackWork(entities.Select(x => x.GetType()), () =>
            {
                entities.ForEach(aEntity =>
                {
                    var targetObj = aEntity.ChangeType(Db.CheckEntityType(aEntity.GetType()));
                    Db.Attach(targetObj);
                    properties.ForEach(aProperty =>
                    {
                        Db.Entry(targetObj).Property(aProperty).IsModified = true;
                    });
                });
            });
        }

        /// <summary>
        /// 按照条件更新记录
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="whereExpre">筛选条件</param>
        /// <param name="set">更新操作</param>
        public void UpdateWhere<T>(Expression<Func<T, bool>> whereExpre, Action<T> set) where T : class, new()
        {
            var list = GetIQueryable<T>().Where(whereExpre).ToList();
            list.ForEach(aData => set(aData));
            Update(list);
        }

        /// <summary>
        /// 使用SQL语句按照条件更新
        /// 用法:UpdateWhere_Sql"Base_User"(x=&gt;x.Id == "Admin",("Name","小明"))
        /// 注：生成的SQL类似于UPDATE [TABLE] SET [Name] = 'xxx' WHERE [Id] = 'Admin'
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="where">筛选条件</param>
        /// <param name="values">字段值设置</param>
        /// <returns>
        /// 影响条数
        /// </returns>
        public int UpdateWhere_Sql<T>(Expression<Func<T, bool>> where, params (string field, object value)[] values) where T : class, new()
        {
            var iq = GetIQueryable<T>().Where(where);

            return _UpdateWhere_Sql(iq, values);
        }

        public int UpdateWhere_Sql(Type entityType, string where, object[] paramters, params (string field, object value)[] values)
        {
            var iq = GetIQueryable(entityType).Where(where, paramters);

            return _UpdateWhere_Sql(iq, values);
        }

        public virtual int _UpdateWhere_Sql(IQueryable iq, params (string field, object value)[] values)
        {
            throw new NotImplementedException("接口未实现");
        }

        #endregion

        #region 查询数据

        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public T GetEntity<T>(params object[] keyValue) where T : class, new()
        {
            var obj = Db.Set<T>().Find(keyValue);
            if (obj != null)
                Db.Entry(obj).State = EntityState.Detached;

            return obj;
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <returns></returns>
        public List<T> GetList<T>() where T : class, new()
        {
            return GetIQueryable<T>().ToList();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns></returns>
        public List<object> GetList(Type type)
        {
            return GetIQueryable(type).CastToList<object>();
        }

        /// <summary>
        /// 获取IQueryable
        /// 注:默认取消实体追踪
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <returns></returns>
        public IQueryable<T> GetIQueryable<T>() where T : class, new()
        {
            return GetIQueryable(typeof(T)) as IQueryable<T>;
        }

        /// <summary>
        /// 获取IQueryable
        /// 注:默认取消实体追踪
        /// </summary>
        /// <param name="type">实体泛型</param>
        /// <returns></returns>
        public IQueryable GetIQueryable(Type type)
        {
            return Db.GetIQueryable(type);
        }

        /// <summary>
        /// 通过SQL获取DataTable
        /// </summary>
        /// <param name="sql">SQL语句(确保语句是安全的)</param>
        /// <returns></returns>
        public DataTable GetDataTableWithSql(string sql)
        {
            return GetDataTableWithSql(sql, null);
        }

        /// <summary>
        /// 通过SQL获取DataTable
        /// </summary>
        /// <param name="sql">SQL语句(确保语句是安全的)</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns></returns>
        public DataTable GetDataTableWithSql(string sql, List<DbParameter> parameters)
        {
            DbProviderFactory dbProviderFactory = NpgsqlFactory.Instance;
            using (DbConnection conn = dbProviderFactory.CreateConnection())
            {
                conn.ConnectionString = ConnectionString;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = 5 * 60;

                    if (parameters != null && parameters?.Count > 0)
                        cmd.Parameters.AddRange(parameters.ToArray());

                    DbDataAdapter adapter = dbProviderFactory.CreateDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataSet table = new DataSet();
                    adapter.Fill(table);
                    cmd.Parameters.Clear();

                    return table.Tables[0];
                }
            }
        }

        /// <summary>
        /// 通过SQL获取List
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="sqlStr">SQL语句</param>
        /// <returns></returns>
        public List<T> GetListBySql<T>(string sqlStr) where T : class, new()
        {
            return GetListBySql<T>(sqlStr, new List<DbParameter>());
        }

        /// <summary>
        /// 通过SQL获取List
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="sqlStr">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns></returns>
        public List<T> GetListBySql<T>(string sqlStr, List<DbParameter> parameters) where T : class, new()
        {
            return Db.Set<T>().FromSql(sqlStr, parameters.ToArray()).ToList();
        }

        #endregion

        #region 执行Sql语句

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        public int ExecuteSql(string sql)
        {
            int count = Db.Database.ExecuteSqlCommand(sql);

            if (!_openedTransaction)
                Dispose();

            return count;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">SQL参数</param>
        public int ExecuteSql(string sql, List<DbParameter> parameters)
        {
            int count = Db.Database.ExecuteSqlCommand(sql, parameters.ToArray());

            if (!_openedTransaction)
                Dispose();

            return count;
        }

        #endregion

        #region 扩展

        public (string sql, IReadOnlyDictionary<string, object> parameters) ToSql(IQueryable query)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _transaction?.Dispose();
                Db?.Dispose();
            }

            _openedTransaction = false;

            _disposed = true;
        }

        ~DbRepository()
        {
            Dispose(false);
        }

        /// <summary>
        /// 执行与释放或重置非托管资源关联的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
