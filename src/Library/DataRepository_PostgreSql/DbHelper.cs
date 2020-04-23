using Library.Extension;
using Library.DataRepository;
using Library.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DbColumn = Library.Models.DbColumn;

namespace Library.DataRepository_PostgreSql
{
    /// <summary>
    /// SqlServer数据库操作帮助类
    /// </summary>
    public class DbHelper : DataRepository.DbHelper
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public DbHelper(string connectionString) : base(connectionString, NpgsqlFactory.Instance)
        {

        }

        #endregion

        #region 私有成员



        #endregion

        #region 外部接口

        [Obsolete("还未完工的方法", true)]
        public override List<DbTableInfo> GetDbTableInfo(string schemaName = null, List<string> table = null, List<string> tableIgnore = null, bool getColumn = false)
        {
            if (schemaName.IsNullOrEmpty())
                return null;
            if (!table.Any())
                return null;
            if (table.Any_Ex(o => o == "*"))
            {
                if (table.Count > 1)
                    table.RemoveAll(o => o != "*");
            }
            else
            {
                if (tableIgnore.Any_Ex())
                {
                    table = table.Where(o => !tableIgnore.Contains(o)).ToList();
                    tableIgnore = null;
                }
            }
            if (!table.Any_Ex())
                return null;

            if (schemaName.IsNullOrEmpty())
                schemaName = "public";

            string sql = string.Format(@"(select 
	                                            relname as ""TableName"",
	                                            cast(obj_description(relfilenode,'pg_class') as varchar) as ""Description""
                                            from pg_class c 
                                            where  relkind = 'r' and relname not like 'pg_%' and relname not like 'sql_%' and relchecks=0
                                            order by relname)

                                            UNION ALL

                                            (SELECT viewname as ""TableName"",NULL as ""Description""
                                            FROM pg_views
                                            WHERE schemaname = '{0}')",
                                        schemaName,
                                        table.Any_Ex() ? $"AND t.table_name IN ({ string.Join(",", table.Select(o => $"'{o}'"))})" : string.Empty,
                                        tableIgnore.Any_Ex() ? $"AND t.table_name NOT IN ({ string.Join(",", tableIgnore.Select(o => $"'{o}'"))})" : string.Empty);

            var result = GetList<DbTableInfo>(sql);
            if (getColumn && result.Any_Ex())
                result.ForEach(o => o.Column = GetDbColumnInfo(schemaName, o.TableName));
            return result;
        }

        [Obsolete("还未完工的方法", true)]
        public override List<DbColumn> GetDbColumnInfo(string schemaName, string tableName)
        {
            string sql = string.Format(@"SELECT 
	                                        a.attname as ""Name"",

                                            pg_type.typname as ""Type"",
                                        (SELECT ""count""(*) from
                                        (SELECT
                                        ic.column_name as ""ColumnName""
                                        FROM
                                        information_schema.table_constraints tc
                                        JOIN information_schema.constraint_column_usage AS ccu USING(constraint_schema, constraint_name)
                                        JOIN information_schema.columns AS ic ON ic.table_schema = tc.constraint_schema AND tc.table_name = ic.table_name AND ccu.column_name = ic.column_name
                                        where constraint_type = 'PRIMARY KEY' and tc.""table_name"" = '{1}') KeyA WHERE KeyA.""ColumnName"" = a.attname)> 0 as ""IsKey"",
                                        a.attnotnull<> True as ""IsNullable"",
	                                        col_description(a.attrelid, a.attnum) as ""Description""


                                        FROM pg_class as c,pg_attribute as a inner join pg_type on pg_type.oid = a.atttypid
                                        where c.relname = '{1}' and a.attrelid = c.oid and a.attnum > 0;",
                        schemaName,
                        tableName);
            var result = GetList<DbColumn>(sql);
            if (result.Any_Ex())
                result.ForEach(o => o.Init(DatabaseType.OdbcDameng));
            return result;
        }

        public override Type DbTypeStr2CsharpType(string dbTypeStr)
        {
            return DbType2CSharpType.GetCSharpType(DatabaseType.PostgreSQL, dbTypeStr);
        }

        public override void SaveEntityToFile(List<DbColumn> infos, string tableName, string tableDescription, string filePath, string nameSpace, string schemaName = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
