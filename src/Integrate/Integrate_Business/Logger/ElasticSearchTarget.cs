using Elasticsearch.Net;
using Integrate_Business.Config;
using Integrate_Entity.Base_Manage;
using Library.Elasticsearch;
using Library.Extension;
using Library.Models;
using Nest;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Integrate_Business
{
    public class ElasticSearchTarget : BaseTarget, ILogSearcher
    {
        #region 构造函数

        static ElasticSearchTarget()
        {
            _elasticHelper = new ElasticsearchHelper<Base_Log>();
        }

        #endregion

        #region 私有成员
        private static ElasticsearchHelper<Base_Log> _elasticHelper { get; set; }
        protected override void Write(LogEventInfo logEvent)
        {
            GetElasticClient().AddOrUpdate(GetBase_SysLogInfo(logEvent));
        }
        private ElasticsearchHelper<Base_Log> GetElasticClient()
        {
            return _elasticHelper;
        }

        #endregion

        #region 外部接口

        public List<Base_Log> GetLogList(
            Pagination pagination,
            string logContent,
            string logType,
            string level,
            string opUserName,
            DateTime? startTime,
            DateTime? endTime)
        {
            var helper = GetElasticClient();
            var filters = new List<Func<QueryContainerDescriptor<Base_Log>, QueryContainer>>();
            if (!logContent.IsNullOrEmpty())
                filters.Add(q => q.Wildcard(w => w.Field(f => f.LogContent).Value($"*{logContent}*")));
            if (!logType.IsNullOrEmpty())
                filters.Add(q => q.Terms(t => t.Field(f => f.LogType).Terms(logType)));
            if (!level.IsNullOrEmpty())
                filters.Add(q => q.Terms(t => t.Field(f => f.Level).Terms(level)));
            if (!opUserName.IsNullOrEmpty())
                filters.Add(q => q.Wildcard(w => w.Field(f => f.CreatorRealName).Value($"*{opUserName}*")));
            if (!startTime.IsNullOrEmpty())
                filters.Add(q => q.DateRange(d => d.Field(f => f.CreateTime).GreaterThan(startTime)));
            if (!endTime.IsNullOrEmpty())
                filters.Add(q => q.DateRange(d => d.Field(f => f.CreateTime).LessThan(endTime)));

            SortOrder sortOrder = pagination.SortType.ToLower() == "asc" ? SortOrder.Ascending : SortOrder.Descending;
            return helper.SearchPaging(Array.Empty<string>(), q => q.Bool(b => b.Filter(filters.ToArray())), pagination);
        }

        #endregion
    }
}
