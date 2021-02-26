using Microservice.Library.DataRepository;
using Microservice.Library.Extension;
using System;
using System.Reflection;

namespace Microservice.Library.DataAccess
{
    /// <summary>
    /// 数据库工厂
    /// <para>请先进行配置</para>
    /// </summary>
    public class DbFactory
    {
        #region 外部接口

        /// <summary>
        /// 根据配置文件获取数据库类型，并返回对应的工厂接口
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="entityAssembly">实体类命名空间</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static IRepository GetRepository(string connectionString = null, string entityAssembly = null, DatabaseType? dbType = null)
        {
            //Type dbRepositoryType = Type.GetType("Library.DataRepository_" + DbTypeToDbTypeStr(dbType ?? DbOption.Option.DbType) + ".DbRepository");
            var _dbType = DbTypeToDbTypeStr(dbType ?? DbOption.Option.DbType);
            var assembly = Assembly.LoadFile($"{AppDomain.CurrentDomain.BaseDirectory}Library.DataRepository_{_dbType}.dll");
            var type = assembly.GetType($"Library.DataRepository_{_dbType}.DbRepository");
            //switch (dbType ?? DbOption.Option.DbType)
            //{
            //    case DatabaseType.SqlServer:
            //        type = typeof(DataRepository_SqlServer.DbRepository);
            //        break;
            //    case DatabaseType.MySql:
            //        type = typeof(DataRepository_MySql.DbRepository);
            //        break;
            //    case DatabaseType.Oracle:
            //        type = typeof(DataRepository_PostgreSql.DbRepository);
            //        break;
            //    case DatabaseType.PostgreSQL:
            //        type = typeof(DataRepository_PostgreSql.DbRepository);
            //        break;
            //    case DatabaseType.OdbcDameng:
            //        type = typeof(DataRepository_DM.DbRepository);
            //        break;
            //    default:
            //        throw new Exception("数据库类型无效");
            //}
            if (type == null)
                throw new Exception("数据库类型无效");
            return Activator.CreateInstance(type, new object[] { connectionString ?? DbOption.Option.ConnectionString, entityAssembly ?? DbOption.Option.EntityAssembly }) as IRepository;
        }

        /// <summary>
        /// 将数据库类型字符串转换为对应的数据库类型
        /// </summary>
        /// <param name="dbTypeStr">数据库类型字符串</param>
        /// <returns></returns>
        public static DatabaseType DbTypeStrToDbType(string dbTypeStr)
        {
            if (dbTypeStr.IsNullOrEmpty())
                throw new Exception("请输入数据库类型字符串！");
            else
            {
                switch (dbTypeStr.ToLower())
                {
                    case "sqlserver": return DatabaseType.SqlServer;
                    case "mysql": return DatabaseType.MySql;
                    case "oracle": return DatabaseType.Oracle;
                    case "postgresql": return DatabaseType.PostgreSQL;
                    case "dm": return DatabaseType.OdbcDameng;
                    default: throw new Exception("请输入合法的数据库类型字符串");
                }
            }
        }

        /// <summary>
        /// 将数据库类型转换为对应的数据库类型字符串
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static string DbTypeToDbTypeStr(DatabaseType dbType)
        {
            if (dbType.IsNullOrEmpty())
                throw new Exception("请输入数据库类型！");
            else
            {
                switch (dbType)
                {
                    case DatabaseType.SqlServer: return "SqlServer";
                    case DatabaseType.MySql: return "MySql";
                    case DatabaseType.Oracle: return "Oracle";
                    case DatabaseType.PostgreSQL: return "PostgreSql";
                    case DatabaseType.OdbcDameng: return "DM";
                    default: throw new Exception("请输入合法的数据库类型！");
                }
            }
        }

        #endregion
    }
}
