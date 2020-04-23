using Library.Extension;
using Library.DataRepository;
using Library.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DbColumn = Library.Models.DbColumn;

namespace Library.DataRepository_MySql
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
        public DbHelper(string connectionString) : base(connectionString, MySqlClientFactory.Instance)
        {

        }

        #endregion

        #region 私有成员



        #endregion

        #region 外部接口

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
											IFNULL((SELECT GROUP_CONCAT(column_name SEPARATOR ';') FROM INFORMATION_SCHEMA.`KEY_COLUMN_USAGE` WHERE table_name = T.TABLE_NAME AND constraint_name = 'PRIMARY'),'') AS PrimaryKey,
											TABLE_NAME AS TableName,
											TABLE_SCHEMA AS SchemaName,
											TABLE_ROWS AS Rows,
											(
										CASE	
											WHEN (
										SELECT
										COUNT( 1 ) 
										FROM
											INFORMATION_SCHEMA.TABLE_CONSTRAINTS C 
										WHERE
											C.TABLE_SCHEMA = T.TABLE_SCHEMA 
											AND C.TABLE_NAME = T.TABLE_NAME 
											AND C.CONSTRAINT_NAME IN ('PRIMARY') 
											) THEN
												1 ELSE 0 
											END > 0 
											) AS HasPrimaryKey,
											TABLE_COMMENT AS Description 
										FROM
											INFORMATION_SCHEMA.`TABLES` T 
										WHERE
											ENGINE = 'InnoDB' 
										AND TABLE_SCHEMA = '{0}' {1} {2}",
                                        schemaName,
                                        table.Any_Ex() ? $"AND t.table_name IN ({ string.Join(",", table.Select(o => $"'{o}'"))})" : string.Empty,
                                        tableIgnore.Any_Ex() ? $"AND t.table_name NOT IN ({ string.Join(",", tableIgnore.Select(o => $"'{o}'"))})" : string.Empty);

            var result = GetList<DbTableInfo>(sql);
            if (getColumn && result.Any_Ex())
                result.ForEach(o => o.Column = GetDbColumnInfo(schemaName, o.TableName));
            return result;
        }

        public override List<DbColumn> GetDbColumnInfo(string schemaName, string tableName)
        {
            string sql = string.Format(@"SELECT 
							                `information_schema`.`COLUMNS`.`COLUMN_NAME` AS `Name`
							                ,`information_schema`.`COLUMNS`.`DATA_TYPE` AS `DataType`
							                ,IFNULL(`information_schema`.`COLUMNS`.`CHARACTER_OCTET_LENGTH`,0) AS ByteLength
							                ,IFNULL(`information_schema`.`COLUMNS`.`CHARACTER_MAXIMUM_LENGTH`,0) AS CharLength
							                ,IFNULL(`information_schema`.`COLUMNS`.`NUMERIC_PRECISION`,0) AS `Precision`
							                ,IFNULL(`information_schema`.`COLUMNS`.`NUMERIC_SCALE`,0) AS Scale
							                ,`information_schema`.`COLUMNS`.`EXTRA` AS IsIdentity
							                ,`information_schema`.`COLUMNS`.`COLUMN_DEFAULT` AS `Default`
							                ,`information_schema`.`COLUMNS`.`IS_NULLABLE` AS IsNullable
							                ,IFNULL(`information_schema`.`COLUMNS`.`COLUMN_COMMENT`,'') AS `Comment`
						                FROM `information_schema`.`COLUMNS`
						                WHERE `information_schema`.`COLUMNS`.`TABLE_SCHEMA` = '{0}'  AND `information_schema`.`COLUMNS`.`TABLE_NAME` = '{1}'",
                        schemaName,
                        tableName);
            var result = GetList<DbColumn>(sql);
            if (result.Any_Ex())
                result.ForEach(o => o.Init(DatabaseType.OdbcDameng));
            return result;
        }

        public override Type DbTypeStr2CsharpType(string dbTypeStr)
        {
            return DbType2CSharpType.GetCSharpType(DatabaseType.MySql, dbTypeStr);
        }

        public override void SaveEntityToFile(List<DbColumn> infos, string tableName, string tableDescription, string filePath, string nameSpace, string schemaName = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
