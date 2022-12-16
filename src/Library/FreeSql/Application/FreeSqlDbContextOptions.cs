using FreeSql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.FreeSql.Application
{
    /// <summary>
    /// 数据库上下文配置
    /// </summary>
    public class FreeSqlDbContextOptions
    {
        public FreeSqlDbContextOptions()
        {
            EnableAddOrUpdateNavigateList = null;
            EntityAssemblys = new List<string>();
            EntityAssemblyFiles = new List<string>();
        }

        /// <summary>
        /// 实体类命名空间集合
        /// </summary>
        public List<string> EntityAssemblys { get; set; }

        /// <summary>
        /// 实体类dll文件集合
        /// </summary>
        public List<string> EntityAssemblyFiles { get; set; }

        /// <summary>
        /// 实体标识
        /// </summary>
        internal string EntityKey { get; set; }

        /// <summary>
        /// 是否开启一对多，多对多级联保存功能
        /// </summary>
        public bool? EnableAddOrUpdateNavigateList { get; set; }

        /// <summary>
        /// 实体变化事件
        /// </summary>
        public Action<List<DbContext.EntityChangeReport.ChangeInfo>> OnEntityChange { get; set; }
    }
}
