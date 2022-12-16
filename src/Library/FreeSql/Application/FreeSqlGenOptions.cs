using FreeSql;
using Microservice.Library.FreeSql;
using System;

namespace Microservice.Library.FreeSql.Application
{
    /// <summary>
    /// 生成配置
    /// </summary>
    public class FreeSqlGenOptions
    {
        /// <summary>
        /// 生成配置
        /// </summary>
        public FreeSqlGeneratorOptions FreeSqlGeneratorOptions { get; set; } = new FreeSqlGeneratorOptions();

        /// <summary>
        /// 开发配置
        /// </summary>
        public FreeSqlDevOptions FreeSqlDevOptions { get; set; } = new FreeSqlDevOptions();

        /// <summary>
        /// 数据库上下文配置
        /// </summary>
        public FreeSqlDbContextOptions FreeSqlDbContextOptions { get; set; } = new FreeSqlDbContextOptions();

        /// <summary>
        /// 设置Builder
        /// </summary>
        public Action<FreeSqlBuilder> SetupBuilder = (builder) => { };
    }
}
