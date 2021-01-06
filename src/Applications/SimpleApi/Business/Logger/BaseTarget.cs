using Entity.System;
using Business.Util;
using NLog;
using NLog.Targets;

namespace Business
{
    public class BaseTarget : TargetWithLayout
    {
        public BaseTarget()
        {
            Name = "系统日志";
            Layout = LoggerConfig.Layout;
        }

        protected System_Log GetBase_SysLogInfo(LogEventInfo logEventInfo)
        {
            var newLog = new System_Log
            {
                Id = IdHelper.NextIdUpper(),
                Data = logEventInfo.Properties[LoggerConfig.Data] as string,
                Level = logEventInfo.Level.ToString(),
                LogContent = logEventInfo.Message,
                LogType = logEventInfo.Properties[LoggerConfig.LogType] as string,
                CreateTime = logEventInfo.TimeStamp,
                OperatorId = logEventInfo.Properties[LoggerConfig.CreatorId] as string,
                OperatorName = logEventInfo.Properties[LoggerConfig.CreatorName] as string
            };

            return newLog;
        }
    }
}
