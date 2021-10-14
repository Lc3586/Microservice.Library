using Dm;
using FreeSql;
using Microservice.Library.FreeSql.Application;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;

namespace Microservice.Library.FreeSql.Annotations
{
    public static class DbParameterExtention
    {
        /// <summary>
        /// 获取模型数据库参数集合
        /// <para>默认为类型名称</para>
        /// </summary>
        /// <typeparam name="TModel">模型类型</typeparam>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public static Dictionary<PropertyInfo, DbParameter> GetDbParameters<TModel>(this TModel model)
            where TModel : class
        {
            var dbParameters = model.GetType()
                                    .GetProperties()
                                    .Where(p => p.IsDefined(typeof(DbParameterAttribute)))
                                    .Select(p => (p, p.GetCustomAttribute<DbParameterAttribute>()))
                                    .OrderBy(p => p.Item2.Order)
                                    .ToDictionary(k => k.p, v => v.Item2.GetDbParameter(v.p, model));

            return dbParameters;
        }

        /// <summary>
        /// 获取属性数据库参数
        /// </summary>
        /// <param name="attribute">特性</param>
        /// <param name="property">属性</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static DbParameter GetDbParameter(this DbParameterAttribute attribute, PropertyInfo property, object obj)
        {
            if (string.IsNullOrEmpty(attribute.Name))
                throw new FreeSqlException($"{property.Name} 属性的 DbParameterAttribute 特性 Name 不可为空");

            DbParameter dbParameter;

            switch (attribute.DataType)
            {
                case DataType.MySql:
                case DataType.OdbcMySql:
                    dbParameter = new MySqlParameter { MySqlDbType = (MySqlDbType)attribute.DbType };
                    break;
                case DataType.PostgreSQL:
                case DataType.OdbcPostgreSQL:
                    dbParameter = new NpgsqlParameter { NpgsqlDbType = (NpgsqlDbType)attribute.DbType };
                    break;
                case DataType.Oracle:
                case DataType.OdbcOracle:
                    dbParameter = new OracleParameter { OracleDbType = (OracleDbType)attribute.DbType };
                    break;
                case DataType.Sqlite:
                    dbParameter = new SQLiteParameter { DbType = (DbType)attribute.DbType };
                    break;
                case DataType.Dameng:
                    dbParameter = new DmParameter { DmSqlType = (DmDbType)attribute.DbType };
                    break;
                case DataType.Odbc:
                case DataType.OdbcKingbaseES:
                case DataType.OdbcDameng:
                case DataType.MsAccess:
                case DataType.ShenTong:
                    dbParameter = new OdbcParameter { OdbcType = (OdbcType)attribute.DbType };
                    break;
                case DataType.OdbcSqlServer:
                case DataType.SqlServer:
                default:
                    dbParameter = new SqlParameter { SqlDbType = (SqlDbType)attribute.DbType };
                    break;
            }

            dbParameter.SetPropertyValue(nameof(dbParameter.ParameterName), attribute.Name)
                    .SetPropertyValue(nameof(dbParameter.Direction), attribute.Direction)
                    .SetPropertyValue(nameof(dbParameter.Value), property.GetValue(obj))
                    .SetPropertyValue(nameof(dbParameter.Size), attribute.Size)
                    .SetPropertyValue(nameof(dbParameter.Scale), attribute.Scale)
                    .SetPropertyValue(nameof(dbParameter.Precision), attribute.Precision)
                    .SetPropertyValue(nameof(dbParameter.IsNullable), attribute.IsNullable);

            return dbParameter;
        }

        /// <summary>
        /// 从模型获取数据库参数
        /// </summary>
        /// <param name="models">模型</param>
        /// <returns>(AllParameters : 全部参数,OutputParameters : 输出参数)</returns>
        internal static (DbParameter[] AllParameters, Dictionary<PropertyInfo, DbParameter> OutputParameters) GetParametersFromModels(this object[] models)
        {
            var allParameter = new Dictionary<PropertyInfo, DbParameter>();
            foreach (var model in models)
            {
                foreach (var parameter in model.GetDbParameters())
                {
                    allParameter.Add(parameter.Key, parameter.Value);
                }
            }

            var allParameterArray = new DbParameter[allParameter.Count()];
            var outParameter = new Dictionary<PropertyInfo, DbParameter>();

            for (int i = 0; i < allParameter.Count(); i++)
            {
                var parameter = allParameter.ElementAt(i);
                allParameterArray[i] = parameter.Value;
                if (parameter.Value.Direction != ParameterDirection.Input)
                    outParameter.Add(parameter.Key, parameter.Value);
            }

            return (allParameterArray, outParameter);
        }

        /// <summary>
        /// 将数据库输出参数的值写入模型
        /// </summary>
        /// <param name="models"></param>
        /// <param name="outputParameters"></param>
        internal static void SetOutputParametersValueToModels(this object[] models, Dictionary<PropertyInfo, DbParameter> outputParameters)
        {
            foreach (var parameter in outputParameters)
            {
                object value = null;

                var property = parameter.Value.Value.GetType().GetProperty("IsNull");
                if (property != null && !(bool)property.GetValue(parameter.Value.Value))
                {
                    var _value = parameter.Value.Value.GetPropertyValue("Value");
                    try
                    {
                        if (parameter.Key.PropertyType.IsGenericType && parameter.Key.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                        {
                            NullableConverter newNullableConverter = new NullableConverter(parameter.Key.PropertyType);
                            value = newNullableConverter.ConvertFrom(_value);
                        }
                        else
                        {
                            value = Convert.ChangeType(_value, parameter.Key.PropertyType);
                        }
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception)
                    {
                        //使用Json序列化
                        value = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(_value), parameter.Key.PropertyType);
                    }
#pragma warning restore CA1031 // Do not catch general exception types
                }

                parameter.Key.SetValue(models.First(m => m.GetType() == parameter.Key.ReflectedType), value);
            }
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        internal static object SetPropertyValue(this object obj, string name, object value)
        {
            obj.GetType().GetProperty(name).SetValue(obj, value);
            return obj;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        internal static object GetPropertyValue(this object obj, string name)
        {
            return obj.GetType().GetProperty(name).GetValue(obj);
        }
    }
}
