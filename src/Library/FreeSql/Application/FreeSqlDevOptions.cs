using FreeSql.Internal;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Microservice.Library.FreeSql.Application
{
    /// <summary>
    /// 开发配置
    /// </summary>
    public class FreeSqlDevOptions
    {
        public FreeSqlDevOptions()
        {

        }

        /// <summary>
        /// 启动时同步实体类型集合到数据库
        /// </summary>
        /// <remarks>默认false</remarks>
        public bool SyncStructureOnStartup { get; set; } = false;

        /// <summary>
        /// 自动同步实体结构到数据库，程序运行中检查实体表是否存在，然后创建或修改
        /// </summary>
        public bool? AutoSyncStructure { get; set; }

        /// <summary>
        /// 实体类名 -> 数据库表名，命名转换（类名、属性名都生效）
        /// 优先级小于 [Table(Name = "xxx")]、[Column(Name = "xxx")]
        /// </summary>
        public NameConvertType? SyncStructureNameConvert { get; set; }

        /// <summary>
        /// 将数据库的主键、自增、索引设置导入，适用 DbFirst 模式，
        /// 无须在实体类型上设置 [Column(IsPrimary)] 或者 ConfigEntity。
        /// 此功能目前可用于 mysql/sqlserver/postgresql/oracle。
        /// </summary>
        public bool? ConfigEntityFromDbFirst { get; set; }
    }
}
