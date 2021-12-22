using FreeSql;
using FreeSql.DatabaseModel;
using Microservice.Library.FreeSql.Annotations;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Library.FreeSql.Extention
{
    /// <summary>
    /// Ado拓展方法
    /// </summary>
    public static class AdoExtension
    {
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="ado">数据库访问对象</param>
        /// <param name="name">存储过程名称</param>
        /// <param name="models">模型</param>
        /// <returns></returns>
        public static int ExecuteStoredProcedureWithModels(this IAdo ado, string name, params object[] models)
        {
            return ado.ExecuteStoredProcedureWithModelsAsync(name, models).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="ado">数据库访问对象</param>
        /// <param name="name">存储过程名称</param>
        /// <param name="models">模型</param>
        /// <returns></returns>
        public async static Task<int> ExecuteStoredProcedureWithModelsAsync(this IAdo ado, string name, params object[] models)
        {
            var parameters = models.GetParametersFromModels();

            var result = await ado.ExecuteStoredProcedureAsync(name, parameters.AllParameters);

            models.SetOutputParametersValueToModels(parameters.OutputParameters);

            return result;
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="ado">数据库访问对象</param>
        /// <param name="name">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static int ExecuteStoredProcedure(this IAdo ado, string name, params DbParameter[] parameters)
        {
            return ado.ExecuteStoredProcedureAsync(name, parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="ado">数据库访问对象</param>
        /// <param name="name">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public async static Task<int> ExecuteStoredProcedureAsync(this IAdo ado, string name, params DbParameter[] parameters)
        {
            return await ado.ExecuteNonQueryAsync(System.Data.CommandType.StoredProcedure, name, parameters);
        }

        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="ado">数据库访问对象</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令</param>
        /// <param name="models">模型</param>
        /// <returns></returns>
        public static int ExecuteNonQueryWithModels(this IAdo ado, System.Data.CommandType cmdType, string cmdText, params object[] models)
        {
            return ado.ExecuteNonQueryWithModelsAsync(cmdType, cmdText, models).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="ado">数据库访问对象</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令</param>
        /// <param name="models">模型</param>
        /// <returns></returns>
        public async static Task<int> ExecuteNonQueryWithModelsAsync(this IAdo ado, System.Data.CommandType cmdType, string cmdText, params object[] models)
        {
            var parameters = models.GetParametersFromModels();

            var result = await ado.ExecuteNonQueryAsync(cmdType, cmdText, parameters.AllParameters);

            models.SetOutputParametersValueToModels(parameters.OutputParameters);

            return result;
        }

        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="ado">数据库访问对象</param>
        /// <param name="name">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(this IAdo ado, string name, params DbParameter[] parameters)
        {
            return ado.ExecuteNonQueryAsync(name, parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="ado">数据库访问对象</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public async static Task<int> ExecuteNonQueryAsync(this IAdo ado, System.Data.CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            return await ado.ExecuteNonQueryAsync(cmdType, cmdText, parameters);
        }

        /// <summary>
        /// 获取数据库名称标识符
        /// </summary>
        /// <param name="ado"></param>
        /// <returns></returns>
        public static char GetCharacter(this IAdo ado)
        {
            switch (ado.DataType)
            {
                case DataType.MySql:
                case DataType.OdbcMySql:
                    return '`';
                case DataType.SqlServer:
                case DataType.PostgreSQL:
                case DataType.Oracle:
                case DataType.Sqlite:
                case DataType.OdbcOracle:
                case DataType.OdbcSqlServer:
                case DataType.OdbcPostgreSQL:
                case DataType.Odbc:
                case DataType.OdbcDameng:
                case DataType.MsAccess:
                case DataType.Dameng:
                case DataType.OdbcKingbaseES:
                case DataType.ShenTong:
                case DataType.KingbaseES:
                case DataType.Firebird:
                case DataType.Custom:
                default:
                    return '"';
            }
        }

        ///// <summary>
        ///// 获取数据库表模式
        ///// </summary>
        ///// <param name="ado"></param>
        ///// <param name="table">表</param>
        ///// <param name="setToDbTableInfo">处理结果保存至DbTableInfo.Schema属性</param>
        ///// <returns></returns>
        //public static string GetDatabaseTableSchema(this IAdo ado, DbTableInfo table, bool setToDbTableInfo = true)
        //{
        //    if (!string.IsNullOrWhiteSpace(table.Schema))
        //        return table.Schema;

        //    //var schema = ado.DataType switch
        //    //{
        //    //    DataType.MySql or
        //    //    DataType.OdbcMySql or
        //    //    DataType.SqlServer or
        //    //    DataType.OdbcSqlServer or
        //    //    DataType.PostgreSQL or
        //    //    DataType.OdbcPostgreSQL => ado.QuerySingle<string>($"SELECT table_schema FROM information_schema.tables WHERE table_name = '{table.Name}'"),
        //    //    DataType.Oracle or
        //    //    DataType.OdbcOracle or
        //    //    DataType.Dameng or
        //    //    DataType.OdbcDameng => ado.QuerySingle<string>($"SELECT OWNER FROM sys.dba_tables WHERE table_name = '{table.Name}'"),
        //    //    DataType.Sqlite or
        //    //    DataType.Odbc or
        //    //    DataType.MsAccess or
        //    //    DataType.OdbcKingbaseES or
        //    //    DataType.ShenTong or
        //    //    DataType.KingbaseES or
        //    //    DataType.Firebird or
        //    //    DataType.Custom or
        //    //   _ => string.Empty
        //    //};

        //    var schema = string.Empty;
        //    switch (ado.DataType)
        //    {
        //        case DataType.MySql:
        //        case DataType.OdbcMySql:
        //        case DataType.SqlServer:
        //        case DataType.OdbcSqlServer:
        //        case DataType.PostgreSQL:
        //        case DataType.OdbcPostgreSQL:
        //            schema = ado.QuerySingle<string>($"SELECT table_schema FROM information_schema.tables WHERE table_name = '{table.Name}'");
        //            break;
        //        case DataType.Oracle:
        //        case DataType.OdbcOracle:
        //        case DataType.Dameng:
        //        case DataType.OdbcDameng:
        //            schema = ado.QuerySingle<string>($"SELECT OWNER FROM sys.dba_tables WHERE table_name = '{table.Name}'");
        //            break;
        //        case DataType.Sqlite:
        //        case DataType.Odbc:
        //        case DataType.MsAccess:
        //        case DataType.OdbcKingbaseES:
        //        case DataType.ShenTong:
        //        case DataType.KingbaseES:
        //        case DataType.Firebird:
        //        case DataType.Custom:
        //        default:
        //            break;
        //    }

        //    if (setToDbTableInfo)
        //        table.Schema = schema;

        //    return schema;
        //}

        /// <summary>
        /// 获取数据库表名
        /// </summary>
        /// <param name="ado"></param>
        /// <param name="table">表</param>
        /// <param name="withCharacter">包含标识符（true: `DBA`.`TableA`, false: DBA.TableA）</param>
        /// <returns></returns>
        public static string GetDatabaseTableName(this IAdo ado, DbTableInfo table, bool withCharacter = true)
        {
            //ado.GetDatabaseTableSchema(table);
            var character = withCharacter ? ado.GetCharacter() : char.MinValue;
            //return $"{character}{table.Schema}{character}.{character}{table.Name}{character}".Replace($"{character}{character}.", "");
            return $"{character}{(new[] { "public", "dbo" }.Contains(table.Schema) ? "" : table.Schema)}{character}.{character}{table.Name}{character}".Replace($"{character}{character}.", "");
        }
    }
}