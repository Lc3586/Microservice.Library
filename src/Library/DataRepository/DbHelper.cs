using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Linq;
using Library.Extention;
using Library.Models;
using DbColumn = Library.Models.DbColumn;

namespace Library.DataRepository
{
    /// <summary>
    /// 数据库帮助类
    /// <!--LCTR 2019-11-14-->
    /// </summary>
    public class DbHelper
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public DbHelper(string connectionString, DbProviderFactory providerFactory)
        {
            ConnectionString = connectionString;
            ProviderFactory = providerFactory;
        }

        #endregion

        #region 私有成员

        /// <summary>
        /// 事务
        /// </summary>
        static readonly Dictionary<int, DbTransaction> Transaction = new Dictionary<int, DbTransaction>();

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="id">事务标识</param>
        /// <param name="il">隔离等级</param>
        protected virtual DbTransaction BeginTrans(int? id = null, IsolationLevel? il = null)
        {
            if (!id.HasValue)
                id = Thread.CurrentThread.ManagedThreadId;
            if (Transaction.ContainsKey(id.Value))
                goto end;
            DbConnection connection = ProviderFactory.CreateConnection();
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            Transaction[id.Value] = il.HasValue ? connection.BeginTransaction(il.Value) : connection.BeginTransaction();
            end:
            return Transaction[id.Value];
        }

        /// <summary>
        /// 结束事务
        /// </summary>
        /// <param name="id">事务标识</param>
        protected virtual void EndTrans(int? id = null)
        {
            if (!id.HasValue)
                id = Thread.CurrentThread.ManagedThreadId;
            Transaction[id.Value].Connection.Close();
            Transaction[id.Value].Connection.Dispose();
            Transaction[id.Value].Dispose();
            Transaction.Remove(id.Value);
        }

        /// <summary>
        /// 从实体提取数据库表名
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <returns></returns>
        protected virtual string GetTableName<T>()
        {
            string table = string.Empty;
            var attributes = typeof(T).GetCustomAttributes(false);
            if (attributes.Length > 0)
                table = attributes.ToList().FirstOrDefault(o => o.GetType().FullName == "System.ComponentModel.DataAnnotations.Schema.TableAttribute")?.GetPropertyValue("Name").ToString();
            return table.IsNullOrEmpty() ? typeof(T).Name : table;
        }

        /// <summary>
        /// 从实体提取主键名
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <returns></returns>
        protected virtual string[] GetPKName<T>()
        {
            string[] table = null;
            var attributes = typeof(T).GetCustomAttributes(false);
            if (attributes.Length > 0)
                table = attributes.ToList().Where(o => o.GetType().FullName == "System.ComponentModel.DataAnnotations.KeyAttribute").Select(o => o.GetPropertyValue("Name").ToString()).ToArray();
            return table.Any() ? table : new[] { "Id" };
        }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <returns></returns>
        protected virtual DbParameter CreateDbParameter(string name, object value, DbType type = DbType.String)
        {
            DbParameter parameter = ProviderFactory.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.DbType = type;
            return parameter;
        }

        #endregion

        #region 外部接口

        #region 字段

        /// <summary>
        /// 驱动
        /// </summary>
        public static DbProviderFactory ProviderFactory;

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        #endregion

        #region 数据

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="id">事务标识(为null时使用当前线程id)</param>
        public virtual void Commit(int? id = null)
        {
            if (!id.HasValue)
                id = Thread.CurrentThread.ManagedThreadId;
            if (!Transaction.ContainsKey(id.Value))
                return;
            Transaction[id.Value].Commit();
            EndTrans(id.Value);
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="id">事务标识(为null时使用当前线程id)</param>
        public virtual void Rollback(int? id = null)
        {
            if (!id.HasValue)
                id = Thread.CurrentThread.ManagedThreadId;
            if (!Transaction.ContainsKey(id.Value))
                return;
            Transaction[id.Value].Rollback();
            EndTrans(id.Value);
        }

        /// <summary>
        /// 执行sql语句并返回DataReader
        /// </summary>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual IDataReader GetDataReader(string sql, bool tran = false, int? transId = null)
        {
            return GetDataReader(sql, null, CommandType.Text, tran, transId);
        }

        /// <summary>
        /// 执行sql语句并返回DataReader
        /// </summary>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="type">命令类型</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual IDataReader GetDataReader(string sql, CommandType type, bool tran = false, int? transId = null)
        {
            return GetDataReader(sql, null, type, tran, transId);
        }

        /// <summary>
        /// 执行sql语句并返回DataReader
        /// </summary>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="parameters">参数</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual IDataReader GetDataReader(string sql, List<DbParameter> parameters, bool tran = false, int? transId = null)
        {
            return GetDataReader(sql, parameters, CommandType.Text, tran, transId);
        }

        /// <summary>
        /// 执行sql语句并返回DataReader
        /// </summary>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="parameters">参数</param>
        /// <param name="type">命令类型</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual IDataReader GetDataReader(string sql, List<DbParameter> parameters, CommandType type, bool tran = false, int? transId = null)
        {
            IDataReader dataReader;
            DbConnection connection;
            using (DbCommand cmd = ProviderFactory.CreateCommand())
            {
                if (tran)
                {
                    cmd.Transaction = BeginTrans(transId.Value);
                    cmd.Connection = cmd.Transaction.Connection;
                }
                else
                    cmd.Connection = ProviderFactory.CreateConnection();
                connection = cmd.Connection;
                connection.ConnectionString = ConnectionString;
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                cmd.CommandText = sql;
                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                cmd.CommandType = type;
                dataReader = cmd.ExecuteReader();
            }
            if (!tran)
            {
                connection.Close();
                connection.Dispose();
            }
            return dataReader;
        }

        /// <summary>
        /// 执行sql语句并返回DataSet
        /// </summary>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="parameters">参数</param>
        /// <param name="type">命令类型</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual DataSet GetDataSet(string sql, bool tran = false, int? transId = null)
        {
            return GetDataSet(sql, null, CommandType.Text, tran, transId);
        }

        /// <summary>
        /// 执行sql语句并返回DataSet
        /// </summary>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="parameters">参数</param>
        /// <param name="type">命令类型</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual DataSet GetDataSet(string sql, CommandType type, bool tran = false, int? transId = null)
        {
            return GetDataSet(sql, null, type, tran, transId);
        }

        /// <summary>
        /// 执行sql语句并返回DataSet
        /// </summary>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="parameters">参数</param>
        /// <param name="type">命令类型</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual DataSet GetDataSet(string sql, List<DbParameter> parameters, bool tran = false, int? transId = null)
        {
            return GetDataSet(sql, parameters, CommandType.Text, tran, transId);
        }

        /// <summary>
        /// 执行sql语句并返回DataSet
        /// </summary>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="parameters">参数</param>
        /// <param name="type">命令类型</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual DataSet GetDataSet(string sql, List<DbParameter> parameters, CommandType type, bool tran = false, int? transId = null)
        {
            DataSet dataset;
            DbConnection connection;
            using (DbCommand cmd = ProviderFactory.CreateCommand())
            {
                if (tran)
                {
                    cmd.Transaction = BeginTrans(transId.Value);
                    cmd.Connection = cmd.Transaction.Connection;
                }
                else
                    cmd.Connection = ProviderFactory.CreateConnection();
                connection = cmd.Connection;
                connection.ConnectionString = ConnectionString;
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                cmd.CommandText = sql;
                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                cmd.CommandType = type;
                DbDataAdapter adapter = ProviderFactory.CreateDataAdapter();
                adapter.SelectCommand = cmd;
                dataset = new DataSet();
                adapter.Fill(dataset);
            }
            if (!tran)
            {
                connection.Close();
                connection.Dispose();
            }
            return dataset;
        }

        /// <summary>
        /// 执行sql语句并返回DataTable
        /// </summary>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual DataTable GetDataTable(string sql, bool tran = false, int? transId = null)
        {
            return GetDataTable(sql, null, CommandType.Text, tran, transId);
        }

        /// <summary>
        /// 执行sql语句并返回DataTable
        /// </summary>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="parameters">参数</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual DataTable GetDataTable(string sql, List<DbParameter> parameters, bool tran = false, int? transId = null)
        {
            return GetDataTable(sql, parameters, CommandType.Text, tran, transId);
        }

        /// <summary>
        /// 执行sql语句并返回DataTable
        /// </summary>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="type">命令类型</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual DataTable GetDataTable(string sql, CommandType type, bool tran = false, int? transId = null)
        {
            return GetDataTable(sql, null, type, tran, transId);
        }

        /// <summary>
        /// 执行sql语句并返回DataTable
        /// </summary>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="parameters">参数</param>
        /// <param name="type">命令类型</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual DataTable GetDataTable(string sql, List<DbParameter> parameters, CommandType type, bool tran = false, int? transId = null)
        {
            return GetDataSet(sql, parameters, type, tran, transId).Tables[0];
        }

        /// <summary>
        /// 执行sql语句并返回List
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(string sql, bool tran = false, int? transId = null) where T : class, new()
        {
            return GetList<T>(sql, null, CommandType.Text, tran, transId);
        }

        /// <summary>
        /// 执行sql语句并返回List
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="type">命令类型</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(string sql, CommandType type, bool tran = false, int? transId = null) where T : class, new()
        {
            return GetList<T>(sql, null, CommandType.Text, tran, transId);
        }

        /// <summary>
        /// 执行sql语句并返回List
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="parameters">参数</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(string sql, List<DbParameter> parameters, bool tran = false, int? transId = null) where T : class, new()
        {
            return GetList<T>(sql, parameters, CommandType.Text, tran, transId);
        }

        /// <summary>
        /// 执行sql语句并返回List
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="parameters">参数</param>
        /// <param name="type">命令类型</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(string sql, List<DbParameter> parameters, CommandType type, bool tran = false, int? transId = null) where T : class, new()
        {
            return GetDataTable(sql, parameters, type, tran, transId).ToList<T>();
        }

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">where条件(确保语句是安全的)</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual List<T> GetEntityList<T>(string where, bool tran = false, int? transId = null) where T : class, new()
        {
            return GetEntityList<T>(where, null, null, null, tran, transId);
        }

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">where条件(确保语句是安全的)</param>
        /// <param name="alias">主表别名</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual List<T> GetEntityList<T>(string where, string alias, bool tran = false, int? transId = null) where T : class, new()
        {
            return GetEntityList<T>(where, alias, null, null, tran, transId);
        }

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">where条件(确保语句是安全的)</param>
        /// <param name="parameters">参数</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual List<T> GetEntityList<T>(string where, List<DbParameter> parameters, bool tran = false, int? transId = null) where T : class, new()
        {
            return GetEntityList<T>(where, null, parameters, null, tran, transId);
        }

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">where条件(确保语句是安全的)</param>
        /// <param name="alias">主表别名</param>
        /// <param name="parameters">参数</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual List<T> GetEntityList<T>(string where, string alias, List<DbParameter> parameters, bool tran = false, int? transId = null) where T : class, new()
        {
            return GetEntityList<T>(where, alias, parameters, null, tran, transId);
        }

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">where条件(确保语句是安全的)</param>
        /// <param name="pagination">分页参数</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual List<T> GetEntityList<T>(string where, Pagination pagination, bool tran = false, int? transId = null) where T : class, new()
        {
            return GetEntityList<T>(where, null, null, pagination, tran, transId);
        }

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">where条件(确保语句是安全的)</param>
        /// <param name="alias">主表别名</param>
        /// <param name="pagination">分页参数</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual List<T> GetEntityList<T>(string where, string alias, Pagination pagination, bool tran = false, int? transId = null) where T : class, new()
        {
            return GetEntityList<T>(where, alias, null, pagination, tran, transId);
        }

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">where条件(确保语句是安全的)</param>
        /// <param name="alias">主表别名</param>
        /// <param name="parameters">参数</param>
        /// <param name="pagination">分页参数</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual List<T> GetEntityList<T>(string where, string alias, List<DbParameter> parameters, Pagination pagination, bool tran = false, int? transId = null) where T : class, new()
        {
            string sql = string.Format("SELECT * FROM {0} {1} WHERE 1=1 AND {2}", GetTableName<T>(), alias, where);
            return GetList<T>(sql, parameters, CommandType.Text, tran, transId);
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">where条件(确保语句是安全的)</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual T GetEntity<T>(string where, bool tran = false, int? transId = null) where T : class, new()
        {
            return GetEntity<T>(where, null, null, tran, transId);
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">where条件(确保语句是安全的)</param>
        /// <param name="alias">主表别名</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual T GetEntity<T>(string where, string alias, bool tran = false, int? transId = null) where T : class, new()
        {
            return GetEntity<T>(where, alias, null, tran, transId);
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">where条件(确保语句是安全的)</param>
        /// <param name="parameters">参数</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual T GetEntity<T>(string where, List<DbParameter> parameters, bool tran = false, int? transId = null) where T : class, new()
        {
            return GetEntity<T>(where, null, parameters, tran, transId);
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">where条件(确保语句是安全的)</param>
        /// <param name="alias">主表别名</param>
        /// <param name="parameters">参数</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual T GetEntity<T>(string where, string alias, List<DbParameter> parameters, bool tran = false, int? transId = null) where T : class, new()
        {
            string sql = string.Format("SELECT * FROM {0} {1} WHERE 1=1 AND {2}", GetTableName<T>(), alias, where);
            return GetList<T>(sql, parameters, CommandType.Text, tran, transId).FirstOrDefault();
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="id">主键值</param>
        /// <returns></returns>
        public virtual T GetEntity<T>(long id) where T : class, new()
        {
            return GetEntity<T>(false, null, id);
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="id">主键值</param>
        /// <returns></returns>
        public virtual T GetEntity<T>(string id) where T : class, new()
        {
            return GetEntity<T>(false, null, id);
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="keyValues">匿名对象键值对</param>
        /// <returns></returns>
        public virtual T GetEntity<T>(params object[] keyValues) where T : class, new()
        {
            return GetEntity<T>(false, null, keyValues);
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <param name="keyValues">匿名对象键值对</param>
        /// <returns></returns>
        public virtual T GetEntity<T>(bool tran, int? transId, params object[] keyValues) where T : class, new()
        {
            string sql = string.Format("SELECT * FROM {0} WHERE Id='{1}'", GetTableName<T>(), keyValues);
            string[] pkNames = GetPKName<T>();
            if (pkNames.Length != keyValues.Length)
                throw new Exception("标识数量不匹配");
            return GetList<T>(sql, pkNames.Select(o => CreateDbParameter(o, keyValues.GetPropertyValue(o))).ToList(), CommandType.Text, tran, transId).FirstOrDefault();
        }

        /// <summary>
        /// 执行sql语句并返回首行首列内容
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual T GetScalar<T>(string sql, bool tran = false, int? transId = null)
        {
            return GetScalar<T>(sql, null, CommandType.Text, tran, transId);
        }

        /// <summary>
        /// 执行sql语句并返回首行首列内容
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="type">命令类型</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual T GetScalar<T>(string sql, CommandType type, bool tran = false, int? transId = null)
        {
            return GetScalar<T>(sql, null, type, tran, transId);
        }

        /// <summary>
        /// 执行sql语句并返回首行首列内容
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="parameters">参数</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual T GetScalar<T>(string sql, List<DbParameter> parameters, bool tran = false, int? transId = null)
        {
            return GetScalar<T>(sql, parameters, CommandType.Text, tran, transId);
        }

        /// <summary>
        /// 执行sql语句并返回首行首列内容
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="parameters">参数</param>
        /// <param name="type">命令类型</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual T GetScalar<T>(string sql, List<DbParameter> parameters, CommandType type, bool tran = false, int? transId = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 执行sql语句并返回受影响的行数
        /// </summary>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual int ExecuteSql(string sql, bool tran = false, int? transId = null)
        {
            return ExecuteSql(sql, null, CommandType.Text, tran, transId);
        }

        /// <summary>
        /// 执行sql语句并返回受影响的行数
        /// </summary>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="type">命令类型</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual int ExecuteSql(string sql, CommandType type, bool tran = false, int? transId = null)
        {
            return ExecuteSql(sql, null, type, tran, transId);
        }

        /// <summary>
        /// 执行sql语句并返回受影响的行数
        /// </summary>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="parameters">参数</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual int ExecuteSql(string sql, List<DbParameter> parameters, bool tran = false, int? transId = null)
        {
            return ExecuteSql(sql, parameters, CommandType.Text, tran, transId);
        }

        /// <summary>
        /// 执行sql语句并返回受影响的行数
        /// </summary>
        /// <param name="sql">Sql语句(确保语句是安全的)</param>
        /// <param name="parameters">参数</param>
        /// <param name="type">命令类型</param>
        /// <param name="tran">是否开启事务</param>
        /// <param name="transId">事务标识(为null时使用当前线程id)</param>
        /// <returns></returns>
        public virtual int ExecuteSql(string sql, List<DbParameter> parameters, CommandType type, bool tran = false, int? transId = null)
        {
            int count = 0;
            using (DbConnection conn = ProviderFactory.CreateConnection())
            {
                conn.ConnectionString = ConnectionString;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (DbCommand cmd = ProviderFactory.CreateCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;

                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (var item in parameters)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }
                    count = cmd.ExecuteNonQuery();
                    return count;
                }
            }
        }

        #endregion

        #region 表结构

        /// <summary>
        /// 获取数据库中的所有表
        /// </summary>
        /// <param name="schemaName">模式（架构）</param>
        /// <param name="table">指定表</param>
        /// <param name="tableIgnore">忽略表</param>
        /// <param name="getColumn">获取列信息</param>
        /// <returns></returns>
        public virtual List<DbTableInfo> GetDbTableInfo(string schemaName = null, List<string> table = null, List<string> tableIgnore = null, bool getColumn = false)
        {
            throw new NotImplementedException("接口未实现");
        }

        /// <summary>
        /// 通过连接字符串和表名获取数据库表的信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public virtual List<DbColumn> GetDbColumnInfo(string tableName)
        {
            throw new NotImplementedException("接口未实现");
        }

        /// <summary>
        /// 通过连接字符串和表名获取数据库表的信息
        /// </summary>
        /// <param name="schemaName">模式（架构）</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public virtual List<DbColumn> GetDbColumnInfo(string schemaName, string tableName)
        {
            throw new NotImplementedException("接口未实现");
        }

        #endregion

        #region 拓展

        /// <summary>
        /// 将数据库类型转为对应C#数据类型
        /// </summary>
        /// <param name="dbTypeStr">数据类型</param>
        /// <returns></returns>
        public virtual Type DbTypeStr2CsharpType(string dbTypeStr)
        {
            throw new NotImplementedException("接口未实现");
        }

        /// <summary>
        /// 生成实体文件
        /// </summary>
        /// <param name="infos">表字段信息</param>
        /// <param name="tableName">表名</param>
        /// <param name="tableDescription">表描述信息</param>
        /// <param name="filePath">文件路径（包含文件名）</param>
        /// <param name="nameSpace">实体命名空间</param>
        /// <param name="schemaName">架构（模式）名</param>
        public virtual void SaveEntityToFile(List<DbColumn> infos, string tableName, string tableDescription, string filePath, string nameSpace, string schemaName = null)
        {
            throw new NotImplementedException("接口未实现");
        }

        #endregion

        #endregion
    }
}
