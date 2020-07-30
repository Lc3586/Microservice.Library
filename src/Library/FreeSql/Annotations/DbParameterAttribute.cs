using FreeSql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Library.FreeSql.Annotations
{
    /// <summary>
    /// 数据库参数特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbParameterAttribute : Attribute
    {
        public DbParameterAttribute()
        {

        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataType DataType { get; set; }

        /// <summary>
        /// 数据库数据类型
        /// </summary>
        public object DbType { get; set; }

        /// <summary>
        /// 参数说明
        /// </summary>
        public ParameterDirection Direction { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 小数位
        /// </summary>
        public byte Scale { get; set; }

        /// <summary>
        /// 精度
        /// </summary>
        public byte Precision { get; set; }

        /// <summary>
        /// 是否可为空
        /// </summary>
        public bool IsNullable { get; set; }
    }
}
