﻿using Dm;
using Microservice.Library.DataRepository;
using Microservice.Library.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Microservice.Library.DataRepository_DM
{
    /// <summary>
    /// DbContext容器
    /// </summary>
    public class RepositoryDbContext : IDisposable
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串或连接字符串</param>
        /// <param name="entityAssembly">数据库实体命名空间,注意,该命名空间应该包含所有需要的数据库实体</param>
        public RepositoryDbContext(string connectionString, string entityAssembly)
        {
            ConnectionString = connectionString;
            EntityAssembly = entityAssembly;
            RefreshDb();
        }

        #endregion

        #region 外部接口

        private static ILoggerFactory _loggerFactory =
            new LoggerFactory(new ILoggerProvider[] { new EFCoreSqlLogeerProvider() });

        public void RefreshDb()
        {
            //重用DbConnection,使用底层相同的DbConnection,支持Model持热更新
            DbConnection con;
            if (_transaction != null)
                con = _transaction.Connection;
            else
            {
                con = _db?.Database?.GetDbConnection();
                if (con == null)
                {
                    con = DmClientFactory.Instance.CreateConnection();
                    con.ConnectionString = ConnectionString;
                }
            }
            if (ModelTypeMap.Count == 0)
                InitModelType();
            _db = new Db(con);
            _db.Database.UseTransaction(_transaction);
            disposedValue = false;
        }

        /// <summary>
        /// 获取模型
        /// </summary>
        /// <param name="type">原类型</param>
        /// <returns></returns>
        public Type GetModel(Type type)
        {
            string modelName = type.Name;

            if (ModelTypeMap.ContainsKey(modelName))
                return ModelTypeMap[modelName];
            else
            {
                ModelTypeMap[modelName] = type;
                RefreshModel();
                return type;
            }
        }

        public DatabaseFacade Database => _db.Database;

        public EntityEntry Entry(object entity)
        {
            var type = entity.GetType();
            var model = CheckModel(entity.GetType());
            object targetObj;
            if (type == model)
                targetObj = entity;
            else
                targetObj = entity.ChangeType_ByConvert(model);

            return _db.Entry(targetObj);
        }

        public DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return _db.Set<TEntity>();
        }

        public IQueryable GetIQueryable(Type type)
        {
            var model = CheckModel(type);
            var dbSet = _db.GetType().GetMethod("Set").MakeGenericMethod(model).Invoke(_db, null);
            var resQ = typeof(EntityFrameworkQueryableExtensions).GetMethod("AsNoTracking").MakeGenericMethod(model).Invoke(null, new object[] { dbSet });
            return resQ as IQueryable;
        }

        public EntityEntry Attach(object entity)
        {
            var type = entity.GetType();
            var model = CheckModel(entity.GetType());
            object targetObj;
            if (type == model)
                targetObj = entity;
            else
                targetObj = entity.ChangeType_ByConvert(model);

            return _db.Attach(targetObj);
        }

        public int SaveChanges()
        {
            return _db.SaveChanges();
        }

        public DbContext GetDbContext()
        {
            return _db;
        }

        public Type CheckEntityType(Type entityType)
        {
            return CheckModel(entityType);
        }

        public void UseTransaction(DbTransaction transaction)
        {
            if (_transaction == transaction)
                return;

            if (_transaction == null && _db.Database.GetDbConnection() == transaction.Connection)
            {
                _transaction = transaction;
            }
            if (_transaction == null && _db.Database.GetDbConnection() != transaction.Connection)
            {
                _transaction = transaction;
                RefreshDb();
            }
        }

        #endregion

        #region 私有成员

        private static ConcurrentDictionary<string, Type> ModelTypeMap { get; } = new ConcurrentDictionary<string, Type>();
        private void InitModelType()
        {
            var assemblies = new Assembly[] { Assembly.Load(EntityAssembly) };
            List<Type> allTypes = new List<Type>();
            assemblies.ForEach(aAssembly =>
            {
                allTypes.AddRange(aAssembly.GetTypes());
            });
            List<Type> types = allTypes
                .Where(x => x.GetCustomAttribute(typeof(TableAttribute), false) != null)
                .ToList();

            types.ForEach(aType =>
            {
                ModelTypeMap[aType.Name] = aType;
            });
        }

        private void RefreshModel()
        {
            RefreshDb();
        }

        private DbTransaction _transaction { get; set; }
        private Db _db { get; set; }

        private class Db : DbContext
        {
            public Db(DbConnection existingConnection)
            {
                _dbConnection = existingConnection;
            }

            private DbConnection _dbConnection { get; }
            private IModel _model { get; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseDm(_dbConnection);
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.UseLoggerFactory(_loggerFactory);
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                ConventionSet conventionSet = DmConventionSetBuilder.Build();
                modelBuilder = new ModelBuilder(conventionSet);
                ModelTypeMap.Values.ForEach(x =>
                {
                    modelBuilder.Model.AddEntityType(x);
                });
            }
        }

        private DatabaseType _dbType { get; }
        private string ConnectionString { get; }
        private string EntityAssembly { get; }
        private Type CheckModel(Type type)
        {
            Type model = GetModel(type);

            return model;
        }
        private Action<string> _HandleSqlLog { get; set; }

        #endregion

        #region Dispose

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _db?.Dispose();
                }
                _transaction = null;
                disposedValue = true;
            }
        }

        ~RepositoryDbContext()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        #endregion
    }
}
