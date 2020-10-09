using Integrate_Business.Config;
using Integrate_Entity.Base_Manage;
using Library.Container;
using Library.Models;
using System;
using System.Collections.Generic;

namespace Integrate_Business.Base_Manage
{
    public class Base_LogBusiness : BaseBusiness<Base_Log>, IBase_LogBusiness, IDependency
    {
        #region 外部接口

        /// <summary>
        /// 获取日志列表
        /// </summary>
        /// <param name="logContent">日志内容</param>
        /// <param name="logType">日志类型</param>
        /// <param name="level">日志级别</param>
        /// <param name="opUserName">操作人用户名</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        public List<Base_Log> GetLogList(
            Pagination pagination,
            string logContent,
            string logType,
            string level,
            string opUserName,
            DateTime? startTime,
            DateTime? endTime)
        {
            ILogSearcher logSearcher = null;

            if (SystemConfig.systemConfig.DefaultLoggerType.HasFlag(LoggerType.RDBMS))
                logSearcher = new RDBMSTarget();
            else if (SystemConfig.systemConfig.DefaultLoggerType.HasFlag(LoggerType.ElasticSearch))
                logSearcher = new ElasticSearchTarget();
            else
                throw new Exception("请指定日志类型为RDBMS或ElasticSearch!");

            return logSearcher.GetLogList(pagination, logContent, logType, level, opUserName, startTime, endTime);
        }

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }
}