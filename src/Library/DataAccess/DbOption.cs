using Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.DataAccess
{
    /// <summary>
    /// Db配置
    /// </summary>
    public class DbOption
    {
        /// <summary>
        /// 配置
        /// </summary>
        internal static DbOption Option = new DbOption();

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="setupAction"></param>
        public static void Configure(Action<DbOption> setupAction)
        {
            setupAction.Invoke(Option);
        }

        /// <summary>
        /// 数据库类型
        /// <para>默认MySql</para>
        /// </summary>
        public DatabaseType DbType { get; set; } = DatabaseType.MySql;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 超时时间
        /// <para>默认5分钟</para>
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// 实体类命名空间
        /// </summary>
        public string EntityAssembly { get; set; }
    }
}
