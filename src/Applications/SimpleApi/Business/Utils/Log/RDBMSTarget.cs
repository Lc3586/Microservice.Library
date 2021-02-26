using Entity.System;
using FreeSql;
using Microservice.Library.Container;
using Microservice.Library.FreeSql.Gen;
using Model.Utils.Config;
using NLog;
using NLog.Targets;

namespace Business.Utils.Log
{
    public class RDBMSTarget : TargetWithLayout
    {
        #region DI

        public RDBMSTarget()
        {

        }

        #endregion

        #region 私有成员

        IBaseRepository<System_Log, string> Repository;

        System_Log GetBase_SysLogInfo(LogEventInfo logEventInfo)
        {
            logEventInfo.Properties.TryGetValue(NLoggerConfig.Data, out object data);
            logEventInfo.Properties.TryGetValue(NLoggerConfig.LogType, out object logType);
            logEventInfo.Properties.TryGetValue(NLoggerConfig.CreatorId, out object creatorId);
            logEventInfo.Properties.TryGetValue(NLoggerConfig.CreatorName, out object creatorName);

            return new System_Log
            {
                Id = IdHelper.NextIdUpper(),
                Data = (string)data,
                Level = logEventInfo.Level.ToString(),
                LogContent = logEventInfo.Message,
                LogType = (string)logType,
                CreateTime = logEventInfo.TimeStamp,
                CreatorId = (string)creatorId,
                CreatorName = (string)creatorName
            };
        }

        protected override async void Write(LogEventInfo logEvent)
        {
            await GetRepository().InsertAsync(GetBase_SysLogInfo(logEvent).InitEntityWithoutOP());
        }

        private IBaseRepository<System_Log, string> GetRepository()
        {
            if (Repository == null)
                Repository = AutofacHelper.GetService<IFreeSqlProvider>()
                                            .GetFreeSql()
                                            .GetRepository<System_Log, string>();
            return Repository;
        }

        #endregion
    }
}
