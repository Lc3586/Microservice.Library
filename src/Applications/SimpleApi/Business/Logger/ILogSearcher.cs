using Entity.System;
using Library.Models;
using System;
using System.Collections.Generic;

namespace Business
{
    public interface ILogSearcher
    {
        List<System_Log> GetLogList(
            Pagination pagination,
            string logContent,
            string logType,
            string level,
            string opUserName,
            DateTime? startTime,
            DateTime? endTime);
    }
}
