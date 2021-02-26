using System;
using System.Collections.Generic;
using System.Diagnostics;
using FreeSql;
using FreeSql.Internal;
using System.Data.Common;

namespace Microservice.Library.FreeSql.Application
{
    /// <summary>
    /// 生成配置
    /// </summary>
    public class FreeSqlGeneratorOptions
    {
        public FreeSqlGeneratorOptions()
        {

        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataType DatabaseType { get; set; }

        /// <summary>
        /// 连接设置
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 处理命令日志
        /// </summary>
        public Action<string> HandleCommandLog { get; set; }

        /// <summary>
        /// 监视数据库命令对象执行前
        /// </summary>
        public Action<DbCommand> MonitorCommandExecuting { get; set; }

        /// <summary>
        /// 监视数据库命令对象执行后，可监视执行性能
        /// </summary>
        public Action<DbCommand, string> MonitorCommandExecuted { get; set; }

        /// <summary>
        /// 延时加载导航属性对象，导航属性需要声明 virtual
        /// </summary>
        public bool? LazyLoading { get; set; }

        /// <summary>
        /// 不使用命令参数化执行，针对 Insert/Update，
        /// 也可临时使用 IInsert/IUpdate.NoneParameter()
        /// </summary>
        public bool? NoneCommandParameter { get; set; }

        /// <summary>
        /// 是否生成命令参数化执行，针对 lambda 表达式解析
        /// </summary>
        public bool? GenerateCommandParameterWithLambda { get; set; }
    }
}
