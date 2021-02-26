using Microservice.Library.Container;
using Microservice.Library.DataRepository;

namespace Microservice.Library.DataAccess
{
    /// <summary>
    /// 描述：数据库帮助类工厂
    /// 作者：Coldairarrow
    /// </summary>
    public class DbHelperFactory
    {
        static DbHelperFactory()
        {
            _container = new IocHelper();
            _container.RegisterType<DbHelper, DataRepository_SqlServer.DbHelper>(DatabaseType.SqlServer.ToString());
            _container.RegisterType<DbHelper, DataRepository_MySql.DbHelper>(DatabaseType.MySql.ToString());
            _container.RegisterType<DbHelper, DataRepository_PostgreSql.DbHelper>(DatabaseType.PostgreSQL.ToString());
            _container.RegisterType<DbHelper, DataRepository_DM.DbHelper>(DatabaseType.OdbcDameng.ToString());
        }

        private static IocHelper _container { get; }

        /// <summary>
        /// 获取指定的数据库帮助类
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="conStr">连接字符串</param>
        /// <returns></returns>
        public static DbHelper GetDbHelper(DatabaseType? dbType = null, string conStr = null)
        {
            return _container.Resolve<DbHelper>((DbFactory.DbTypeToDbTypeStr(dbType ?? DbOption.Option.DbType)), conStr ?? DbOption.Option.ConnectionString);
        }

        /// <summary>
        /// 获取指定的数据库帮助类
        /// </summary>
        /// <param name="dbTypeStr">数据库类型字符串</param>
        /// <param name="conStr">连接字符串</param>
        /// <returns></returns>
        public static DbHelper GetDbHelper(string dbTypeStr, string conStr)
        {
            DatabaseType dbType = DbFactory.DbTypeStrToDbType(dbTypeStr);
            return GetDbHelper(dbType, conStr);
        }
    }
}
