using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.DataRepository
{
    /// <summary>
    /// 数据库数据类型转C#数据类型
    /// <!--LCTR 2019-06-10-->
    /// </summary>
    public class DbType2CSharpType
    {
        /// <summary>
        /// 获取数据库数据类型对应的C#数据类型名称
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="dataType">数据库数据类型</param>
        /// <returns></returns>
        public static string GetCSharpTypeName(DatabaseType dbType, string dataType)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    return GetCsharpTypeName_SQLServer(dataType);
                case DatabaseType.Oracle:
                case DatabaseType.PostgreSQL:
                case DatabaseType.MySql:
                default:
                    return GetCSharpTypeName_MySQL(dataType);
            }
        }

        /// <summary>
        /// 获取数据库数据类型对应的C#数据类型
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="dataType">数据库数据类型</param>
        /// <returns></returns>
        public static Type GetCSharpType(DatabaseType dbType, string dataType)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    return GetCsharpType_SQLServer(dataType);
                case DatabaseType.Oracle:
                case DatabaseType.PostgreSQL:
                case DatabaseType.MySql:
                default:
                    return GetCSharpType_MySQL(dataType);
            }
        }

        /// <summary>
        /// 获取MySQL数据库数据类型对应的C#数据类型名称
        /// </summary>
        /// <param name="dataType">数据库数据类型</param>
        /// <returns></returns>
        private static string GetCSharpTypeName_MySQL(string dataType)
        {
            switch (dataType.ToLower())
            {
                case "tinyint":
                    return "sbyte";
                case "smallint":
                    return "short";
                case "mediumint":
                case "int":
                case "integer":
                    return "int";
                case "double":
                    return "double";
                case "float":
                    return "float";
                case "decimal":
                case "numeric":
                    return "decimal";
                case "real":
                    return "Single";
                case "bit":
                    return "bool";
                case "time":
                    return "TimeSpan";
                case "year":
                    return "int";
                case "date":
                case "datetime":
                case "timestamp":
                    return "DateTime";
                case "tinyblob":
                case "blob":
                case "mediumblob":
                case "longblog":
                case "binary":
                case "varbinary":
                    return "byte[]";
                case "char":
                case "varchar":
                case "tinytext":
                case "text":
                case "mediumtext":
                case "longtext":
                    return "string";
                case "point":
                case "linestring":
                case "polygon":
                case "geometry":
                case "multipoint":
                case "multilinestring":
                case "multipolygon":
                case "geometrycollection":
                case "enum":
                case "set":
                default:
                    return "object";
            }
        }

        /// <summary>
        /// 获取MySQL数据库数据类型对应的C#数据类型
        /// </summary>
        /// <param name="dataType">数据库数据类型</param>
        /// <returns></returns>
        private static Type GetCSharpType_MySQL(string dataType)
        {
            switch (dataType)
            {
                case "tinyint":
                    return typeof(sbyte);
                case "smallint":
                    return typeof(short);
                case "mediumint":
                case "int":
                case "integer":
                    return typeof(int);
                case "double":
                    return typeof(double);
                case "float":
                    return typeof(float);
                case "decimal":
                case "numeric":
                    return typeof(decimal);
                case "real":
                    return typeof(Single);
                case "bit":
                    return typeof(bool);
                case "time":
                    return typeof(TimeSpan);
                case "year":
                    return typeof(int);
                case "date":
                case "datetime":
                case "timestamp":
                    return typeof(DateTime);
                case "tinyblob":
                case "blob":
                case "mediumblob":
                case "longblog":
                case "binary":
                case "varbinary":
                    return typeof(byte[]);
                case "char":
                case "varchar":
                case "tinytext":
                case "text":
                case "mediumtext":
                case "longtext":
                    return typeof(string);
                case "point":
                case "linestring":
                case "polygon":
                case "geometry":
                case "multipoint":
                case "multilinestring":
                case "multipolygon":
                case "geometrycollection":
                case "enum":
                case "set":
                default:
                    return typeof(object);
            }
        }

        /// <summary>
        /// 获取SQLServer数据库数据类型对应的C#数据类型名称
        /// </summary>
        /// <param name="dataType">数据库数据类型</param>
        /// <returns></returns>
        private static string GetCsharpTypeName_SQLServer(string dataType)
        {
            if (string.IsNullOrEmpty(dataType)) return dataType;
            dataType = dataType.ToLower();
            string csharpType = "object";
            switch (dataType)
            {
                case "bigint": csharpType = "long"; break;
                case "binary": csharpType = "byte[]"; break;
                case "bit": csharpType = "bool"; break;
                case "char": csharpType = "string"; break;
                case "date": csharpType = "DateTime"; break;
                case "datetime": csharpType = "DateTime"; break;
                case "datetime2": csharpType = "DateTime"; break;
                case "datetimeoffset": csharpType = "DateTimeOffset"; break;
                case "decimal": csharpType = "decimal"; break;
                case "float": csharpType = "double"; break;
                case "image": csharpType = "byte[]"; break;
                case "int": csharpType = "int"; break;
                case "money": csharpType = "decimal"; break;
                case "nchar": csharpType = "string"; break;
                case "ntext": csharpType = "string"; break;
                case "numeric": csharpType = "decimal"; break;
                case "nvarchar": csharpType = "string"; break;
                case "real": csharpType = "Single"; break;
                case "smalldatetime": csharpType = "DateTime"; break;
                case "smallint": csharpType = "short"; break;
                case "smallmoney": csharpType = "decimal"; break;
                case "sql_variant": csharpType = "object"; break;
                case "sysname": csharpType = "object"; break;
                case "text": csharpType = "string"; break;
                case "time": csharpType = "TimeSpan"; break;
                case "timestamp": csharpType = "byte[]"; break;
                case "tinyint": csharpType = "sbyte"; break;
                case "uniqueidentifier": csharpType = "Guid"; break;
                case "varbinary": csharpType = "byte[]"; break;
                case "varchar": csharpType = "string"; break;
                case "xml": csharpType = "string"; break;
                default: csharpType = "object"; break;
            }
            return csharpType;
        }

        /// <summary>
        /// 获取SQLServer数据库数据类型对应的C#数据类型
        /// </summary>
        /// <param name="dataType">数据库数据类型</param>
        /// <returns></returns>
        private static Type GetCsharpType_SQLServer(string dataType)
        {
            if (string.IsNullOrEmpty(dataType)) return Type.Missing.GetType();
            dataType = dataType.ToLower();
            Type commonType = typeof(object);
            switch (dataType)
            {
                case "bigint": commonType = typeof(long); break;
                case "binary": commonType = typeof(byte[]); break;
                case "bit": commonType = typeof(bool); break;
                case "char": commonType = typeof(string); break;
                case "date": commonType = typeof(DateTime); break;
                case "datetime": commonType = typeof(DateTime); break;
                case "datetime2": commonType = typeof(DateTime); break;
                case "datetimeoffset": commonType = typeof(DateTimeOffset); break;
                case "decimal": commonType = typeof(decimal); break;
                case "float": commonType = typeof(double); break;
                case "image": commonType = typeof(byte[]); break;
                case "int": commonType = typeof(int); break;
                case "money": commonType = typeof(decimal); break;
                case "nchar": commonType = typeof(string); break;
                case "ntext": commonType = typeof(string); break;
                case "numeric": commonType = typeof(decimal); break;
                case "nvarchar": commonType = typeof(string); break;
                case "real": commonType = typeof(Single); break;
                case "smalldatetime": commonType = typeof(DateTime); break;
                case "smallint": commonType = typeof(short); break;
                case "smallmoney": commonType = typeof(decimal); break;
                case "sql_variant": commonType = typeof(object); break;
                case "sysname": commonType = typeof(object); break;
                case "text": commonType = typeof(string); break;
                case "time": commonType = typeof(TimeSpan); break;
                case "timestamp": commonType = typeof(byte[]); break;
                case "tinyint": commonType = typeof(sbyte); break;
                case "uniqueidentifier": commonType = typeof(Guid); break;
                case "varbinary": commonType = typeof(byte[]); break;
                case "varchar": commonType = typeof(string); break;
                case "xml": commonType = typeof(string); break;
                default: commonType = typeof(object); break;
            }
            return commonType;
        }
    }
}
