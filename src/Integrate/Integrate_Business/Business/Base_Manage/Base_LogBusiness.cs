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
        #region �ⲿ�ӿ�

        /// <summary>
        /// ��ȡ��־�б�
        /// </summary>
        /// <param name="logContent">��־����</param>
        /// <param name="logType">��־����</param>
        /// <param name="level">��־����</param>
        /// <param name="opUserName">�������û���</param>
        /// <param name="startTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="pagination">��ҳ����</param>
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
                throw new Exception("��ָ����־����ΪRDBMS��ElasticSearch!");

            return logSearcher.GetLogList(pagination, logContent, logType, level, opUserName, startTime, endTime);
        }

        #endregion

        #region ˽�г�Ա

        #endregion

        #region ����ģ��

        #endregion
    }
}