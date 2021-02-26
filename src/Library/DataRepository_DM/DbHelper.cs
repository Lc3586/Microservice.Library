using Dm;
using Microservice.Library.DataRepository;
using Microservice.Library.Extension;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservice.Library.DataRepository_DM
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
        public DbHelper(string connectionString) : base(connectionString, DmClientFactory.Instance)
        {

        }

        #endregion

        #region 私有成员



        #endregion

        #region 外部接口

        /// <summary>
        /// 获取数据库中的所有表
        /// </summary>
        /// <param name="schemaName">模式（架构）</param>
        /// <param name="table">指定表</param>
        /// <param name="tableIgnore">忽略表</param>
        /// <param name="getColumn">获取列信息</param>
        /// <returns></returns>
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

            string sql = string.Format(@"SELECT 
                                            t.table_name,
                                            t.tablespace_name schemname,
                                            t.num_rows ""rows"",
                                            CAST
                                            (
                                                CASE
                                                    WHEN(SELECT COUNT(1) FROM user_constraints tc WHERE tc.table_name = t.table_name AND tc.CONSTRAINT_TYPE = 'P') >= 1 THEN 1
                                                    ELSE 0
                                                END
                                             AS BIT) HasPrimaryKey,
                                            ISNULL(c.COMMENTS, t.table_name) ""remark""
                                            FROM user_tables t
                                            LEFT JOIN user_tab_comments c ON c.table_name = t.table_name
                                            WHERE t.tablespace_name = '{0}' {1} {2}",
                                        schemaName,
                                        table.Any_Ex() ? $"AND t.table_name IN ({ string.Join(",", table.Select(o => $"'{o}'"))})" : string.Empty,
                                        tableIgnore.Any_Ex() ? $"AND t.table_name NOT IN ({ string.Join(",", tableIgnore.Select(o => $"'{o}'"))})" : string.Empty);

            var result = GetList<DbTableInfo>(sql);
            if (getColumn && result.Any_Ex())
                result.ForEach(o => o.Column = GetDbColumnInfo(schemaName, o.TableName));
            return result;
        }

        /// <summary>
        /// 通过连接字符串和表名获取数据库表的信息
        /// </summary>
        /// <param name="schemaName">模式（架构）</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public override List<DbColumn> GetDbColumnInfo(string schemaName, string tableName)
        {
            string sql = string.Format(@"WITH CONS AS (
	                                        SELECT
		                                        con.CONSTRAINT_TYPE,
		                                        con.TABLE_NAME,
		                                        concol.COLUMN_NAME,
		                                        O_R_TAB.NAME R_TABLENAME
	                                        FROM 
		                                        user_constraints con,
		                                        user_cons_columns concol,
		                                        SYS.SYSOBJECTS O_CON,
		                                        SYS.SYSOBJECTS O_R_TAB
	                                        WHERE 
		                                        con.TABLE_NAME = concol.TABLE_NAME AND
		                                        con.CONSTRAINT_NAME = concol.CONSTRAINT_NAME AND
		                                        O_CON.NAME = con.INDEX_NAME AND
		                                        O_R_TAB.ID = O_CON.PID
                                        )
                                        SELECT
                                        c.COLUMN_ID ColumnId,
                                        c.COLUMN_NAME Name,
                                        c.DATA_TYPE datatype,
                                        CAST
                                        (
                                            CASE
                                                WHEN (SELECT COUNT(1) FROM CONS con WHERE con.TABLE_NAME = c.TABLE_NAME AND con.COLUMN_NAME = c.COLUMN_NAME AND con.CONSTRAINT_TYPE = 'P') > 0 THEN 1
                                                ELSE 0
                                            END
                                         AS BIT) IsPrimaryKey,
                                        CAST
                                        (
                                            CASE
                                                WHEN (SELECT COUNT(1) FROM CONS con WHERE con.TABLE_NAME = c.TABLE_NAME AND con.COLUMN_NAME = c.COLUMN_NAME AND con.CONSTRAINT_TYPE = 'R') > 0 THEN 1
                                                ELSE 0
                                            END
                                         AS BIT) IsForeignKey,
                                        (SELECT WM_CONCAT(con.R_TABLENAME) FROM CONS con WHERE con.TABLE_NAME = c.TABLE_NAME AND con.COLUMN_NAME = c.COLUMN_NAME AND con.CONSTRAINT_TYPE = 'R') FKTableNames,
                                        c.DATA_LENGTH ByteLength,
                                        c.CHAR_LENGTH CharLength,
                                        IFNULL(c.data_precision,0) ""Precision"",
                                        c.DATA_SCALE Scale,
                                        c.DATA_DEFAULT ""Default"",
                                        CAST
                                        (
	                                        CASE
		                                        WHEN c.NULLABLE = 'Y' THEN 1
		                                        ELSE 0
	                                        END
                                         AS BIT) IsNullable,
                                        colcom.COMMENTS  ""Comment""
                                        FROM all_tab_columns c
                                        LEFT JOIN user_col_comments colcom ON colcom.COLUMN_NAME = c.COLUMN_NAME AND colcom.TABLE_NAME = c.TABLE_NAME
                                        WHERE c.TABLE_NAME = '{1}' AND c.OWNER = 'DMHR'
                                        ORDER BY c.COLUMN_ID",
                        schemaName,
                        tableName);
            var result = GetList<DbColumn>(sql);
            if (result.Any_Ex())
                result.ForEach(o => o.Init(DatabaseType.OdbcDameng));
            return result;
        }

        /// <summary>
        /// 将数据库类型转为对应C#数据类型
        /// </summary>
        /// <param name="dbTypeStr">数据类型</param>
        /// <returns></returns>
        public override Type DbTypeStr2CsharpType(string dbTypeStr)
        {
            return DbType2CSharpType.GetCSharpType(DatabaseType.OdbcDameng, dbTypeStr);
        }

        public override void SaveEntityToFile(List<DbColumn> infos, string tableName, string tableDescription, string filePath, string nameSpace, string schemaName = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
