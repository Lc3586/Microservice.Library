using Library.Models;

namespace Model.System
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DatabaseConfig
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
        public DatabaseType DatabaseType { get; set; }

        /// <summary>
        /// 默认数据库连接字符串
        /// </summary>
        public string ConnectString { get; set; }

        /// <summary>
        /// 实体类命名空间
        /// </summary>
        public string EntityAssembly { get; set; }
    }
}
