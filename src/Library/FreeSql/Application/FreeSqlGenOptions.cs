using Library.FreeSql;
using System;

namespace Library.FreeSql.Application
{
    /// <summary>
    /// 生成配置
    /// </summary>
    public class FreeSqlGenOptions
    {
        public FreeSqlGeneratorOptions FreeSqlGeneratorOptions { get; set; } = new FreeSqlGeneratorOptions();

        public FreeSqlDevOptions FreeSqlDevOptions { get; set; } = new FreeSqlDevOptions();

        public FreeSqlDbContextOptions FreeSqlDbContextOptions { get; set; } = new FreeSqlDbContextOptions();
    }
}
