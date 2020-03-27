using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Library.FreeSql.Application
{
    /// <summary>
    /// 开发选项
    /// </summary>
    public class FreeSqlDevOptions
    {
        public FreeSqlDevOptions()
        {

        }

        /// <summary>
        /// 启动时同步实体类型集合到数据库
        /// </summary>
        public bool? SyncStructureOnStartup { get; set; }

        /// <summary>
        /// 自动同步实体结构到数据库，程序运行中检查实体表是否存在，然后创建或修改
        /// </summary>
        public bool? AutoSyncStructure { get; set; }

        /// <summary>
        /// 转小写同步结构
        /// </summary>
        public bool? SyncStructureToLower { get; set; }

        /// <summary>
        /// 转大写同步结构
        /// </summary>
        public bool? SyncStructureToUpper { get; set; }

        /// <summary>
        /// 将数据库的主键、自增、索引设置导入，适用 DbFirst 模式，
        /// 无须在实体类型上设置 [Column(IsPrimary)] 或者 ConfigEntity。
        /// 此功能目前可用于 mysql/sqlserver/postgresql/oracle。
        /// </summary>
        public bool? ConfigEntityFromDbFirst { get; set; }
    }
}
