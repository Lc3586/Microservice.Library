using Entity.System;
using Library.Container;
using Library.Elasticsearch;
using Library.Elasticsearch.Gen;
using Model.Utils.Config;
using NLog;
using NLog.Targets;

namespace Business.Utils.Log
{
    public class ElasticSearchTarget : TargetWithLayout
    {
        #region DI

        public ElasticSearchTarget()
        {

        }

        #endregion

        #region 私有成员

        ElasticsearchClient Elasticsearch;

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
            await GetElasticClient().AddAsync(GetBase_SysLogInfo(logEvent));
        }

        private ElasticsearchClient GetElasticClient()
        {
            if (Elasticsearch == null)
                Elasticsearch = AutofacHelper.GetService<IElasticsearchProvider>()
                                            .GetElasticsearch<System_Log>();
            return Elasticsearch;
        }

        #endregion
    }
}
