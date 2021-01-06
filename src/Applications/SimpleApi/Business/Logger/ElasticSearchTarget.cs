using Elasticsearch.Net;
using Entity.System;
using Library.Elasticsearch;
using Library.Elasticsearch.Gen;
using Library.Extension;
using Library.Models;
using Nest;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business
{
    public class ElasticSearchTarget : BaseTarget, ILogSearcher
    {
        #region DI

        public ElasticSearchTarget(IElasticsearchProvider elasticsearchProvider)
        {
            elasticsearch = elasticsearchProvider.GetElasticsearch<System_Log>();
        }

        #endregion

        #region 私有成员

        private static ElasticsearchClient elasticsearch { get; set; }

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
            Pagination pagination,
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
                filters.Add(q => q.Wildcard(w => w.Field(f => f.OperatorName).Value($"*{opUserName}*")));
            if (!startTime.IsNullOrEmpty())
                filters.Add(q => q.DateRange(d => d.Field(f => f.CreateTime).GreaterThan(startTime)));
            if (!endTime.IsNullOrEmpty())
                filters.Add(q => q.DateRange(d => d.Field(f => f.CreateTime).LessThan(endTime)));

            SortOrder sortOrder = pagination.SortType.ToString() == "asc" ? SortOrder.Ascending : SortOrder.Descending;
            return helper.SearchPaging<System_Log>(Array.Empty<string>(), q => q.Bool(b => b.Filter(filters.ToArray())), pagination);
        }

        #endregion
    }
}
