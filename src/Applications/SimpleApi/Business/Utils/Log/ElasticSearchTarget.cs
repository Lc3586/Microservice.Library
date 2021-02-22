using Entity.System;
using Library.Container;
using Library.Elasticsearch;
using Library.Elasticsearch.Gen;
using Library.Extension;
using Model.Utils.Config;
using Model.Utils.Pagination;
using Nest;
using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;

namespace Business.Utils.Log
{
    public class ElasticSearchTarget : TargetWithLayout
    {
        #region DI

        public ElasticSearchTarget()
        {
            elasticsearch = AutofacHelper.GetScopeService<IElasticsearchProvider>()
                                        .GetElasticsearch<System_Log>();
        }

        #endregion

        #region 私有成员

        readonly ElasticsearchClient elasticsearch;

        System_Log GetBase_SysLogInfo(LogEventInfo logEventInfo)
        {
            return new System_Log
            {
                Id = IdHelper.NextIdUpper(),
                Data = logEventInfo.Properties[NLoggerConfig.Data] as string,
                Level = logEventInfo.Level.ToString(),
                LogContent = logEventInfo.Message,
                LogType = logEventInfo.Properties[NLoggerConfig.LogType] as string,
                CreateTime = logEventInfo.TimeStamp,
                CreatorId = logEventInfo.Properties[NLoggerConfig.CreatorId] as string,
                CreatorName = logEventInfo.Properties[NLoggerConfig.CreatorName] as string
            };
        }

        protected override void Write(LogEventInfo logEvent)
        {
            GetElasticClient().AddOrUpdate(GetBase_SysLogInfo(logEvent));
        }

        private ElasticsearchClient GetElasticClient()
        {
            return elasticsearch;
        }

        #endregion
    }
}
