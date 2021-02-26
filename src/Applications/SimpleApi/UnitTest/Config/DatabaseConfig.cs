using System;
using System.Collections.Generic;
using System.Text;
using Entity.Example;
using Microservice.Library.Configuration;

namespace UnitTest.Config
{
    /// <summary>
    /// 数据库测试数据
    /// </summary>
    public class DatabaseConfig
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public static DatabaseConfig GetData()
        {
            return new ConfigHelper("jsonconfig/database.json").GetModel<DatabaseConfig>("Datas");
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        public List<Sample_DB> Insert { get; set; }

        /// <summary>
        /// 修改数据
        /// </summary>
        public List<Sample_DB> Update { get; set; }
    }
}
