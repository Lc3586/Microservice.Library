using Library.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using T4CAGC.Models;

namespace T4CAGC.Template
{
    /// <summary>
    /// 生成实体model
    /// </summary>
    public partial class Model_Entity
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        public GenerateConfig config;

        /// <summary>
        /// 表格信息
        /// </summary>
        public DbTableInfo dbTableInfo;

        public Model_Entity(GenerateConfig _config, DbTableInfo _dbTableInfo)
        {
            config = _config;
            dbTableInfo = _dbTableInfo;
        }
    }
}
