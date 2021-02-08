using Entity.System;
using Library.Container;
using Library.Elasticsearch;
using Library.Elasticsearch.Gen;
using Library.Extension;
using Model.System.Config;
using Model.System.Pagination;
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

        #region 外部接口

        public List<System_Log> GetLogList(
            PaginationDTO pagination,
            string logContent,
            string logType,
            string level,
            string opUserName,
            DateTime? startTime,
            DateTime? endTime)
        {
            var helper = GetElasticClient();
            var filters = new List<Func<QueryContainerDescriptor<System_Log>, QueryContainer>>();
            if (!logContent.IsNullOrEmpty())
                filters.Add(q => q.Wildcard(w => w.Field(f => f.LogContent).Value($"*{logContent}*")));
            if (!logType.IsNullOrEmpty())
                filters.Add(q => q.Terms(t => t.Field(f => f.LogType).Terms(logType)));
            if (!level.IsNullOrEmpty())
                filters.Add(q => q.Terms(t => t.Field(f => f.Level).Terms(level)));
            if (!opUserName.IsNullOrEmpty())
                filters.Add(q => q.Wildcard(w => w.Field(f => f.CreatorName).Value($"*{opUserName}*")));
            if (!startTime.IsNullOrEmpty())
                filters.Add(q => q.DateRange(d => d.Field(f => f.CreateTime).GreaterThan(startTime)));
            if (!endTime.IsNullOrEmpty())
                filters.Add(q => q.DateRange(d => d.Field(f => f.CreateTime).LessThan(endTime)));

            SortOrder sortOrder = pagination.SortType.ToString() == "asc" ? SortOrder.Ascending : SortOrder.Descending;

            var result = helper.SearchPaging<System_Log>(out long recordCount,
                Array.Empty<string>(),
                q => q.Bool(b => b.Filter(filters.ToArray())),
                so => so.Field(typeof(System_Log).GetProperty(pagination.SortField), pagination.SortType == SortType.asc ? SortOrder.Ascending : SortOrder.Descending),
                pagination.PageIndex,
                pagination.PageRows);

            pagination.RecordCount = recordCount;

            return result;
        }

        #endregion
    }
}
