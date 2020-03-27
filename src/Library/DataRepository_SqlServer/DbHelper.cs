using Library.Extention;
using Library.DataRepository;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DbColumn = Library.Models.DbColumn;
using System.Data.SqlClient;

namespace Library.DataRepository_SqlServer
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
        public DbHelper(string connectionString) : base(connectionString, SqlClientFactory.Instance)
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
                                            obj.name tablename,
                                            schem.name schemname,
                                            idx.rows,
                                            CAST
                                            (
                                                CASE 
                                                    WHEN (SELECT COUNT(1) FROM sys.indexes WHERE object_id= obj.OBJECT_ID AND is_primary_key=1) >=1 THEN 1
                                                    ELSE 0
                                                END 
                                            AS BIT) HasPrimaryKey,
									        isnull(g.[value],obj.name) as Remark                                     
                                            from [{0}].sys.objects obj 
                                            inner join [{0}].dbo.sysindexes idx on obj.object_id=idx.id and idx.indid<=1
                                            INNER JOIN [{0}].sys.schemas schem ON obj.schema_id=schem.schema_id
									        left join sys.extended_properties g on (obj.object_id = g.major_id AND g.minor_id = 0)
                                            where type='U' {1} {2}
                                            order by obj.name",
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
            string sql = string.Format(@"WITH indexCTE AS
                                        (
                                            SELECT 
                                            ic.column_id,
                                            ic.index_column_id,
                                            ic.object_id    
                                            FROM [{0}].sys.indexes idx
                                            INNER JOIN [{0}].sys.index_columns ic ON idx.index_id = ic.index_id AND idx.object_id = ic.object_id
                                            WHERE  idx.object_id =OBJECT_ID('{1}') AND idx.is_primary_key=1
                                        )
                                        select
                                        colm.column_id ColumnID,
                                        CAST(CASE WHEN indexCTE.column_id IS NULL THEN 0 ELSE 1 END AS BIT) IsPrimaryKey,
                                        colm.name ""Name"",
                                        systype.name ""DataType"",
                                        colm.is_identity IsIdentity,
                                        colm.is_nullable IsNullable,
                                        cast(colm.max_length as int) ByteLength,
                                        (
                                            case 
                                                when systype.name='nvarchar' and colm.max_length>0 then colm.max_length/2 
                                                when systype.name='nchar' and colm.max_length>0 then colm.max_length/2
                                                when systype.name='ntext' and colm.max_length>0 then colm.max_length/2 
                                                else colm.max_length
                                            end
                                        ) CharLength,
                                        cast(colm.precision as int) Precision,
                                        cast(colm.scale as int) Scale,
                                        isnull(prop.value,colm.name) ""Comment""
                                        from [{0}].sys.columns colm
                                        inner join [{0}].sys.types systype on colm.system_type_id=systype.system_type_id and colm.user_type_id=systype.user_type_id
                                        left join [{0}].sys.extended_properties prop on colm.object_id=prop.major_id and colm.column_id=prop.minor_id
                                        LEFT JOIN indexCTE ON colm.column_id=indexCTE.column_id AND colm.object_id=indexCTE.object_id                                        
                                        where colm.object_id=OBJECT_ID('{1}')
                                        order by colm.column_id",
                        schemaName,
                        tableName);
            var result = GetList<DbColumn>(sql);
            if (result.Any_Ex())
                result.ForEach(o => o.Init(DatabaseType.OdbcDameng));
            return result;
        }

        public override Type DbTypeStr2CsharpType(string dbTypeStr)
        {
            return DbType2CSharpType.GetCSharpType(DatabaseType.SqlServer, dbTypeStr);
        }

        public override void SaveEntityToFile(List<DbColumn> infos, string tableName, string tableDescription, string filePath, string nameSpace, string schemaName = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
