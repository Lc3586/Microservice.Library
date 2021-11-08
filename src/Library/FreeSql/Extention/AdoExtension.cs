using FreeSql;
using Microservice.Library.FreeSql.Annotations;
using System.Data.Common;
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
    }
}