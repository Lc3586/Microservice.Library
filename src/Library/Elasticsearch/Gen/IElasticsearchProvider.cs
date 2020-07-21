using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Elasticsearch.Gen
{
    public interface IElasticsearchProvider
    {
        /// <summary>
        /// 获取搜索服务对象
        /// </summary>
        /// <typeparam name="T">索引类型</typeparam>
        /// <param name="state">日期(分库功能参数,如未指定,默认为当前日期）</param>
        /// <returns></returns>
        ElasticsearchClient GetElasticsearch<T>(DateTime? state = null) where T : class;
    }
}
