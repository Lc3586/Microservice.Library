using FreeSql.Internal;

namespace Model.Utils.Config
{
    /// <summary>
    /// FreeSql配置
    /// </summary>
    public class FreeSqlSetting
    {
        /// <summary>
        /// 数据库缓存过滤（不将相同的变更提交数据库）
        /// </summary>
        public bool DbCacheFilter { get; set; }

        /// <summary>
        /// 自动同步实体结构到数据库，程序运行中检查实体表是否存在，然后创建或修改
        /// </summary>
        public bool AutoSyncStructure { get; set; }

        /// <summary>
        /// 实体类名 -> 数据库表名，命名转换（类名、属性名都生效）
        /// 优先级小于 [Table(Name = "xxx")]、[Column(Name = "xxx")]
        /// </summary>
        public NameConvertType? SyncStructureNameConvert { get; set; }

        /// <summary>
        /// 启动时同步实体类型集合到数据库
        /// </summary>
        public bool SyncStructureOnStartup { get; set; }
    }
}
