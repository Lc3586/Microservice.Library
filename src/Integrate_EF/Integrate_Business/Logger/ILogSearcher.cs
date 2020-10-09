using Integrate_Entity.Base_Manage;
using Library.Models;
using System;
using System.Collections.Generic;

namespace Integrate_Business
{
    public interface ILogSearcher
    {
        List<Base_Log> GetLogList(
            Pagination pagination,
            string logContent,
            string logType,
            string level,
            string opUserName,
            DateTime? startTime,
            DateTime? endTime);
    }
}
