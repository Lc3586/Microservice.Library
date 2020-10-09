﻿using Integrate_Entity.Base_Manage;
using Library.DataAccess;
using Library.Extension;
using Library.Models;
using Library.LinqTool;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Integrate_Business
{
    public class RDBMSTarget : BaseTarget, ILogSearcher
    {
        public List<Base_Log> GetLogList(
            Pagination pagination,
            string logContent,
            string logType,
            string level,
            string opUserName,
            DateTime? startTime,
            DateTime? endTime)
        {
            using (var db = DbFactory.GetRepository())
            {
                var whereExp = Where.True<Base_Log>();
                if (!logContent.IsNullOrEmpty())
                    whereExp = whereExp.And(x => x.LogContent.Contains(logContent));
                if (!logType.IsNullOrEmpty())
                    whereExp = whereExp.And(x => x.LogType == logType);
                if (!level.IsNullOrEmpty())
                    whereExp = whereExp.And(x => x.Level == level);
                if (!opUserName.IsNullOrEmpty())
                    whereExp = whereExp.And(x => x.CreatorRealName.Contains(opUserName));
                if (!startTime.IsNullOrEmpty())
                    whereExp = whereExp.And(x => x.CreateTime >= startTime);
                if (!endTime.IsNullOrEmpty())
                    whereExp = whereExp.And(x => x.CreateTime <= endTime);

                return db.GetIQueryable<Base_Log>().Where(whereExp).GetPagination(pagination).ToList();
            }
        }

        protected override void Write(LogEventInfo logEvent)
        {
            using (var db = DbFactory.GetRepository())
            {
                db.Insert(GetBase_SysLogInfo(logEvent));
            }
        }
    }
}
