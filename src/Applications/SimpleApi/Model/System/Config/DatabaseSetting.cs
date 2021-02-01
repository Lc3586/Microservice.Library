using FreeSql;
using System.Collections.Generic;

namespace Model.System.Config
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DatabaseSetting
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataType DatabaseType { get; set; }

        /// <summary>
        /// 默认数据库连接字符串
        /// </summary>
        public string ConnectString { get; set; }

        /// <summary>
        /// 实体类命名空间
        /// </summary>
        public List<string> EntityAssembly { get; set; }
    }
}
