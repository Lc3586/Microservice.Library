using Elasticsearch.Net;
using Microservice.Library.Elasticsearch.Application;
using Microservice.Library.Extension;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace Microservice.Library.Elasticsearch
{
    /// <summary>
    /// Elasticsearch搜索服务
    /// <!--LCTR 2019-07-29-->
    /// <para>v7.x</para>
    /// </summary>
    public class ElasticsearchClient
    {
        /// <summary>
        /// 客户端
        /// </summary>
        public static ElasticClient ElasticClient { get; set; }

        /// <summary>
        /// 关系名
        /// </summary>
        public string RelationName { get; set; }

        /// <summary>
        /// 索引名
        /// </summary>

        public string IndiceName { get; set; }

        /// <summary>
        /// 获取ES客户端
        /// </summary>
        /// <returns></returns>
        public static ElasticClient GetClient()
        {
            return ElasticClient;
        }

        /// <summary>
        /// 索引是否存在
        /// </summary>
        /// <param name="indices">索引</param>
        /// <returns></returns>
#pragma warning disable CA1822 // Mark members as static
        public bool ExistsIndices(Indices indices)
#pragma warning disable CA1822 // Mark members as static
        {
            return ElasticClient.Indices.Exists(indices).Exists;
        }

        /// <summary>
        /// 更新索引设置
        /// </summary>
        /// <param name="indices">索引</param>
        /// <param name="selector">设置</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool UpdateIndicesSettings(
            Indices indices,
            Func<UpdateIndexSettingsDescriptor, IUpdateIndexSettingsRequest> selector,
            bool isThrow = false)
        {
            ElasticClient.Indices.Close(indices);
            var response = ElasticClient.Indices.UpdateSettings(indices, selector);
            ElasticClient.Indices.Open(indices);
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 创建别名
        /// </summary>
        /// <param name="alias">别名</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool CreateAlias(
            string alias,
            bool isThrow = false)
        {
            var aliasExists = ElasticClient.Indices.AliasExists(alias, s => s.Index(IndiceName));
            if (!aliasExists.IsValid && aliasExists.ApiCall?.HttpStatusCode != (int)HttpStatusCode.NotFound)
            {
                if (isThrow)
                    throw new ElasticsearchException(aliasExists);
                else
                    return false;
            }
            if (aliasExists.Exists)
                return true;

            var aliasPut = ElasticClient.Indices.PutAlias(IndiceName, alias, s => s.Index(IndiceName));
            if (!aliasPut.IsValid && isThrow)
                throw new ElasticsearchException(aliasPut);
            return aliasPut.IsValid;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool Add<T>(
            T model,
            bool isThrow = false) where T : class
        {
            var response = ElasticClient.Index(model, s => s.Index(IndiceName));
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public async Task<bool> AddAsync<T>(
            T model,
            bool isThrow = false) where T : class
        {
            var response = await ElasticClient.IndexAsync(model, s => s.Index(IndiceName));
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="models">数据集合</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool Add<T>(
            List<T> models,
            bool isThrow = false) where T : class
        {
            var response = ElasticClient.Index(models, s => s.Index(IndiceName));
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="models">数据集合</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool AddBulk<T>(
            List<T> models,
            bool isThrow = false) where T : class
        {
            var response = ElasticClient.Bulk(b => b.Index(IndiceName).CreateMany(models));
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 新增或更新
        /// 注意：此方法将使用默认索引
        /// </summary>
        /// <param name="model">数据</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool AddOrUpdate<T>(
            T model,
            bool isThrow = false) where T : class
        {
            //var response = elasticClient.DocumentExists(new DocumentPath<T>(model).Index(relationName));
            //if (!response.IsValid && isThrow)
            ////    throw new ElasticsearchError(response);

            //return response.Exists ? Update(model, isThrow) : Add(model, isThrow);

            var response = ElasticClient.Index(model, s => s.Index(IndiceName));
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 批量新增或更新
        /// 注意：此方法将使用默认索引
        /// </summary>
        /// <param name="models">数据集合</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool AddOrUpdate<T>(
            List<T> models,
            bool isThrow = false) where T : class
        {
            var response = ElasticClient.Index(models, s => s.Index(IndiceName));
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 批量新增或更新
        /// 注意：此方法将使用默认索引
        /// </summary>
        /// <param name="models">数据集合</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool AddOrUpdateBulk<T>(
            List<T> models,
            bool isThrow = false) where T : class
        {
            var response = ElasticClient.Bulk(b => b.Index(IndiceName).IndexMany(models));
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 删除
        /// 注意：此方法将使用默认索引
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool Delete<T>(
            object id,
            bool isThrow = false) where T : class
        {
            var response = ElasticClient.Delete(new DocumentPath<T>(new Id(id)).Index(IndiceName));
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 批量删除
        /// 注意：此方法将使用默认索引
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool DeleteBulk<T>(
            List<long> ids,
            bool isThrow = false) where T : class
        {
            var response = ElasticClient.Bulk(b => b.Index(IndiceName).DeleteMany<T>(ids));
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 批量删除
        /// 注意：此方法将使用默认索引
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool DeleteBulk<T>(
            List<string> ids,
            bool isThrow = false) where T : class
        {
            var response = ElasticClient.Bulk(b => b.Index(IndiceName).DeleteMany<T>(ids));
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 批量删除
        /// 注意：此方法将使用默认索引
        /// </summary>
        /// <param name="models">数据集合</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool DeleteBulk<T>(
            List<T> models,
            bool isThrow = false) where T : class
        {
            var response = ElasticClient.Bulk(b => b.Index(IndiceName).DeleteMany<T>(models));
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 局部更新
        /// 注意：此方法将使用默认索引
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="partial">局部数据</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool Update<T>(
            object id,
            object partial,
            bool isThrow = false) where T : class
        {
            var response = ElasticClient.Update<T, object>(new DocumentPath<T>(new Id(id)).Index(IndiceName), p => p.Doc(partial));
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 批量局部更新
        /// 注意：此方法将使用默认索引
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <param name="partial">局部数据</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool UpdateBulk<T>(
            List<object> ids,
            object partial,
            bool isThrow = false) where T : class
        {
            if (ids == null)
                return false;
            var Bulk = new BulkDescriptor().Index(IndiceName);
            for (int i = 0; i < ids.Count; i++)
                Bulk.Update<T, object>(u => u.Id(new Id(ids[i])).Doc(partial));
            var response = ElasticClient.Bulk(Bulk);
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 批量局部更新
        /// 注意：此方法将使用默认索引
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <param name="partials">局部数据集合</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool UpdateBulk<T>(
            List<object> ids,
            List<object> partials,
            bool isThrow = false) where T : class
        {
            if (ids == null || partials == null)
                return false;
            if (ids.Count != partials.Count)
                throw new Exception("集合数量不一致");
            var Bulk = new BulkDescriptor().Index(IndiceName);
            for (int i = 0; i < ids.Count; i++)
                Bulk.Update<T, object>(u => u.Id(new Id(ids[i])).Doc(partials[i]));
            var response = ElasticClient.Bulk(Bulk);
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 批量局部更新
        /// 注意：此方法将使用默认索引
        /// </summary>
        /// <param name="models">数据集合</param>
        /// <param name="partial">局部数据</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool UpdateBulk<T>(
            List<T> models,
            object partial,
            bool isThrow = false) where T : class
        {
            if (models == null)
                return false;
            var response = ElasticClient.Bulk(b => b.Index(IndiceName).UpdateMany<T, object>(models, (u1, u2) => u1.IdFrom(u2).Doc(partial)));
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 批量局部更新
        /// 注意：此方法将使用默认索引
        /// </summary>
        /// <param name="models">数据集合</param>
        /// <param name="partial">局部数据集合</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool UpdateBulk<T>(
            List<T> models,
            List<object> partial,
            bool isThrow = false) where T : class
        {
            if (models == null || partial == null)
                return false;
            if (models.Count != partial.Count)
                throw new Exception("集合数量不一致");
            var Bulk = new BulkDescriptor().Index(IndiceName);
            for (int i = 0; i < models.Count; i++)
                Bulk.Update<T, object>(u => u.IdFrom(models[i]).Doc(partial[i]));
            var response = ElasticClient.Bulk(Bulk);
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 整体更新
        /// 注意：此方法将使用默认索引
        /// </summary>
        /// <param name="model">数据</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool Update<T>(
            T model,
            bool isThrow = false) where T : class
        {
            var response = ElasticClient.Update(new DocumentPath<T>(Id.From(model)).Index(IndiceName), p => p.Doc(model));
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 批量整体更新
        /// 注意：此方法将使用默认索引
        /// </summary>
        /// <param name="models">数据集合</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public bool UpdateBulk<T>(
            List<T> models,
            bool isThrow = false) where T : class
        {
            var response = ElasticClient.Bulk(b => b.Index(IndiceName).UpdateMany<T>(models, (u1, u2) => u1.IdFrom(u2).Doc(u2)));
            if (!response.IsValid && isThrow)
                throw new ElasticsearchException(response);
            return response.IsValid;
        }

        /// <summary>
        /// 获取数据
        /// 注意：此方法将使用默认索引
        /// </summary>
        /// <param name="indice">索引</param>
        /// <param name="id">ID</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public static T Get<T>(
            string indice,
            object id,
            bool isThrow = false) where T : class
        {
            //var response = ElasticClient.Get(new DocumentPath<T>(new Id(id)), s => s.Index(RelationName));
            var response = ElasticClient.Get<T>(new GetRequest(indice, new Id(id)));
            if (!response.IsValid)
                if (isThrow)
                    throw new ElasticsearchException(response);
                else
                    return null;
            return response.Source;
        }

        /// <summary>
        /// 获取数据
        /// 注意：此方法将使用默认索引
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public T Get<T>(
            object id,
            bool isThrow = false) where T : class
        {
            var response = ElasticClient.Get<T>(new GetRequest(IndiceName, new Id(id)));
            if (!response.IsValid)
                if (isThrow)
                    throw new ElasticsearchException(response);
                else
                    return null;
            return response.Source;
        }

        /// <summary>
        /// 获取数据
        /// 注意：此方法将使用默认索引
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public T Get<T, TValue>(
            Expression<Func<T, TValue>> field,
            object value,
            bool isThrow = false) where T : class
        {
            var response = ElasticClient.Search<T>(s =>
               s.Size(1)
               .Index(IndiceName)
               .Query(q =>
                   q.Bool(b =>
                       b.Must(m =>
                           m.Term(qs =>
                               qs.Field(field).Value(value))))));

            if (!response.IsValid)
                if (isThrow)
                    throw new ElasticsearchException(response);
                else
                    return null;

            return response.Documents?.CastToList<T>()?.FirstOrDefault();
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public static List<T> Search<T>(
            Func<SearchDescriptor<T>, ISearchRequest> selector = null,
            bool isThrow = false) where T : class
        {
            var response = ElasticClient.Search<T>(selector);
            if (!response.IsValid)
                if (isThrow)
                    throw new ElasticsearchException(response);
                else
                    return null;
            return response.Documents.CastToList<T>();
        }

        /// <summary>
        /// 查询数据量
        /// </summary>
        /// <param name="indices">指定索引(null:默认，[]:全部,["i_0","i_1","i_2"]:指定)</param>
        /// <param name="query">查询条件</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public long Count<T>(
            string[] indices = null,
            Func<QueryContainerDescriptor<T>, QueryContainer> query = null,
            bool isThrow = false) where T : class
        {
            bool Transfinite = Total<T>() > 10000;
            var response = GetSearch(indices, query, null, Transfinite, null, false);
            if (!response.IsValid)
            {
                if (isThrow)
                    throw new ElasticsearchException(response);
                else
                    return 0L;
            }
            return response.Total;
        }

        /// <summary>
        /// 获取查询
        /// </summary>
        /// <param name="indices">指定索引(null:默认，[]:全部,["i_0","i_1","i_2"]:指定)</param>
        /// <param name="query">查询条件</param>
        /// <param name="sort">排序(key:字段名,mode:方式(默认desc))</param>
        /// <param name="time">快照保存时间(秒,默认60)</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public ISearchResponse<T> GetSearch<T>(
            string[] indices = null,
            Func<QueryContainerDescriptor<T>, QueryContainer> query = null,
            Dictionary<string, string> sort = null,
            int time = 60,
            bool isThrow = false) where T : class
        {
            bool Transfinite = Total<T>() > 10000;
            string _time = ConvertTime(time);
            var response = GetSearch(indices, query, sort, Transfinite, _time, true);
            if (!response.IsValid)
            {
                if (isThrow)
                    throw new ElasticsearchException(response);
                else
                    return null;
            }
            return response;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="search"></param>
        /// <param name="max"></param>
        /// <param name="isThrow"></param>
        /// <returns></returns>
        public List<T> GetList<T>(
            ISearchResponse<T> search,
            int? max = null,
            bool isThrow = false) where T : class
        {
            List<T> data = new List<T>();
            bool Transfinite = Total<T>() > 10000;
            if (Transfinite)
            {
                while (true)
                {
                    if (!search.IsValid || search.ScrollId == null)
                    {
                        if (isThrow)
                            throw new Exception(search.ServerError.Error.Reason);
                        break;
                    }

                    if (!search.Documents.Any_Ex())
                        break;

                    data.AddRange(search.Documents);

                    if (max != null && data.Count >= max)
                        break;

                    search = ElasticClient.Scroll<T>("2m", search.ScrollId);
                }
            }
            else
            {
                if (!search.IsValid)
                {
                    if (isThrow)
                        throw new Exception(search.ServerError.Error.Reason);
                }
                else
                    data.AddRange(search.Documents);
            }

            ElasticClient.ClearScroll(new ClearScrollRequest(search.ScrollId));

            return data;
        }

        /// <summary>
        /// 查询(大数据量)
        /// </summary>
        /// <param name="indices">指定索引(null:默认，[]:全部,["i_0","i_1","i_2"]:指定)</param>
        /// <param name="query">查询条件</param>
        /// <param name="max">最大数据量</param>
        /// <param name="sort">排序(key:字段名,mode:方式(默认desc))</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public List<T> SearchLarge<T>(
            string[] indices = null,
            Func<QueryContainerDescriptor<T>, QueryContainer> query = null,
            int? max = null,
            Dictionary<string, string> sort = null,
            bool isThrow = false) where T : class
        {
            List<T> data = new List<T>();
            return GetList(GetSearch(indices, query, sort, 0, true), max, isThrow);
        }

        /// <summary>
        /// 查询(分页)
        /// </summary>
        /// <param name="recordCount">总数据量(不分页)</param>
        /// <param name="indices">指定索引(null:默认，[]:全部,["i_0","i_1","i_2"]:指定)</param>
        /// <param name="query"></param>
        /// <param name="sort">排序</param>
        /// <param name="pageIndex">指定页码</param>//(仅支持在数据量小于等于10000时进行分页)
        /// <param name="pageRows">每页数据量</param>
        /// <param name="time">快照保存时间(秒,默认60)</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public List<T> SearchPaging<T>(
            out long recordCount,
            string[] indices = null,
            Func<QueryContainerDescriptor<T>, QueryContainer> query = null,
            Func<SortDescriptor<T>, IPromise<IList<ISort>>> sort = null,
            int? pageIndex = null,
            int? pageRows = null,
            int time = 60,
            bool isThrow = false) where T : class
        {
            //if (pageIndex != null && pageIndex * pageRows > 10000)
            //    throw new Exception("数据量过大");

            var indices_ = GetIndices(indices);

            var _time = TimeSpan.FromSeconds(time);
            List<T> data = new List<T>();
            bool Transfinite = false;
            var response = ElasticClient.Search<T>(s =>
            {
                s = s.Index(indices_);
                if (query != null)
                    s = s.Query(query);
                else
                    s = s.MatchAll();

                if (pageIndex != null)
                {
                    s = s.Size(pageRows)
                         .From((pageIndex - 1) * pageRows);
                }
                else
                    Transfinite = Total<T>() > 10000;

                if (sort != null)
                    s = s.Sort(sort);

                if (Transfinite)
                    s = s.Scroll(_time);
                return s;
            });

            recordCount = response.Total;

            if (Transfinite)
            {
                while (true)
                {
                    if (!response.IsValid || response.ScrollId == null)
                    {
                        if (isThrow)
                            throw new ElasticsearchException(response);
                        break;
                    }

                    if (!response.Documents.Any_Ex())
                        break;

                    data.AddRange(response.Documents);
                    response = ElasticClient.Scroll<T>(_time, response.ScrollId);
                }
            }
            else
            {
                if (!response.IsValid)
                {
                    if (isThrow)
                        throw new ElasticsearchException(response);
                }
                else
                    data.AddRange(response.Documents);
            }

            ElasticClient.ClearScroll(new ClearScrollRequest(response.ScrollId));

            return data;
        }

        /// <summary>
        /// 查询(sql)
        /// </summary>
        /// <param name="recordCount">总数据量(不分页)</param>
        /// <param name="indices">指定索引(null:默认，[]:全部,["i_0","i_1","i_2"]:指定)</param>
        /// <param name="query">sql查询语句</param>
        /// <param name="size">数据量</param>
        /// <param name="time">快照保存时间(秒,默认60)</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public List<T> SearchWithSql<T>(
            out long recordCount,
            string[] indices = null,
            string query = null,
            int? size = null,
            int time = 60,
            bool isThrow = false) where T : class
        {
            //if (pageIndex != null && pageIndex * pageRows > 10000)
            //    throw new Exception("数据量过大");

            var indices_ = GetIndices(indices);

            var _time = TimeSpan.FromSeconds(time);
            List<T> data = new List<T>();

            var sql = $"select * from {GetIndex(indices)} {query}";

            if (size != null)
                sql += $" limit {size}";

            bool Transfinite = Total(indices_) > 10000;

            var request = new TranslateSqlRequest()
            {
                Query = sql,
                FetchSize = size
            };
            var respone_sql = ElasticClient.LowLevel.Sql.Translate<StringResponse>(PostData.Serializable(request));
            var query_Dsl = respone_sql.Body;
            var requestParameters = Transfinite ? new SearchRequestParameters() { Scroll = _time } : null;
            ISearchResponse<T> respone_search = ElasticClient.LowLevel.Search<SearchResponse<T>>(GetIndex(indices), query_Dsl, requestParameters);

            recordCount = respone_search.Total;

            if (Transfinite)
            {
                while (true)
                {
                    if (!respone_search.IsValid || respone_search.ScrollId == null)
                    {
                        if (isThrow)
                            throw new Exception(respone_search.ServerError.Error.Reason);
                        break;
                    }

                    if (!respone_search.Documents.Any_Ex())
                        break;

                    data.AddRange(respone_search.Documents);
                    respone_search = ElasticClient.Scroll<T>(_time, respone_search.ScrollId);
                }
            }
            else
            {
                if (!respone_search.IsValid)
                {
                    if (isThrow)
                        throw new Exception(respone_search.ServerError.Error.Reason);
                }
                else
                    data.AddRange(respone_search.Documents);
            }

            ElasticClient.ClearScroll(new ClearScrollRequest(respone_search.ScrollId));

            return data;
        }

        /// <summary>
        /// 查询(游标)
        /// </summary>
        /// <param name="indices">指定索引(null:默认，[]:全部,["i_0","i_1","i_2"]:指定)</param>
        /// <param name="query"></param>
        /// <param name="scrollId">滚动ID</param>
        /// <param name="time">快照保存时间(秒,默认60)</param>
        /// <param name="size">数据量上限</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="sortType">排序类型(desc,asc[默认])</param>
        /// <param name="isThrow">抛出异常</param>
        /// <returns></returns>
        public List<T> SearchScroll<T>(
            string[] indices = null,
            Func<QueryContainerDescriptor<T>, QueryContainer> query = null,
            string scrollId = null,
            int time = 60,
            int? size = null,
            string sortField = null,
            string sortType = "asc",
            bool isThrow = false) where T : class
        {
            var indices_ = GetIndices(indices);

            string _time = ConvertTime(time);
            List<T> data = new List<T>();
            bool Transfinite = size > 10000 && Total<T>() > 10000;
            var response = scrollId != null ? ElasticClient.Scroll<T>(_time, scrollId) : ElasticClient.Search<T>(s =>
                {
                    s = s.Index(indices_);

                    if (query != null)
                        s = s.Query(query);
                    else
                        s = s.MatchAll();
                    if (size != null)
                    {
                        s = s.Size(size)
                             .Sort(so => so.Field(typeof(T).GetProperty(sortField), sortType == "asc" ? SortOrder.Ascending : SortOrder.Descending));
                    }
                    if (Transfinite)
                        s = s.Scroll(_time);
                    return s;
                });

            if (Transfinite)
            {
                while (true)
                {
                    if (!response.IsValid || response.ScrollId == null)
                    {
                        if (isThrow)
                            throw new ElasticsearchException(response);
                        break;
                    }

                    if (!response.Documents.Any_Ex())
                        break;

                    data.AddRange(response.Documents);
                    response = ElasticClient.Scroll<T>(_time, response.ScrollId);
                }
            }
            else
            {
                if (!response.IsValid)
                {
                    if (isThrow)
                        throw new ElasticsearchException(response);
                }
                else
                    data.AddRange(response.Documents);
            }

            ElasticClient.ClearScroll(new ClearScrollRequest(response.ScrollId));

            return data;
        }

        /// <summary>
        /// 默认索引的总数据量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
#pragma warning disable CA1822 // Mark members as static
        public long Total<T>(
#pragma warning restore CA1822 // Mark members as static
            QueryContainer query = null
            ) where T : class
        {
            var response = ElasticClient.Count<T>(s => s.Query(q => query));
            if (!response.IsValid)
                throw new ElasticsearchException(response);
            return response.Count;
        }

        /// <summary>
        /// 指定索引的总数据量
        /// </summary>
        /// <returns></returns>
        public long Total(
            string[] indices = null,
            QueryContainer query = null)
        {
            return Total(GetIndices(indices), query);
        }

        /// <summary>
        /// 指定索引的总数据量
        /// </summary>
        /// <param name="indices"></param>
        /// <param name="query"></param>
        /// <returns></returns>
#pragma warning disable CA1822 // Mark members as static
        public long Total(
#pragma warning disable CA1822 // Mark members as static
            Indices indices,
            QueryContainer query = null)
        {
            var request = new CountRequest(indices);
            if (query != null)
                request.Query = query;
            var response = ElasticClient.Count(request);
            if (!response.IsValid)
                throw new ElasticsearchException(response);
            return response.Count;
        }

        /// <summary>
        /// 指定索引的总数据量
        /// </summary>
        /// <param name="indices"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public long TotalWithSql(
            string[] indices = null,
            string query = null)
        {
            var request = new TranslateSqlRequest()
            {
                Query = query
            };
            var respone_sql = ElasticClient.LowLevel.Sql.Translate<StringResponse>(PostData.Serializable(request));
            var query_Dsl = respone_sql.Body;
            var respone_count = ElasticClient.LowLevel.Count<CountResponse>(GetIndex(indices), query_Dsl);

            if (!respone_count.IsValid)
                throw new ElasticsearchException(respone_count);
            return respone_count.Count;
        }

        /// <summary>
        /// 获取索引
        /// </summary>
        /// <param name="indices">指定索引(null:默认，[]:全部(拥有相同别名的索引),["i_0","i_1","i_2"]:指定索引)</param>
        /// <returns></returns>
        private Indices GetIndices(string[] indices)
        {
            if (indices == null)
                return Indices.Index(IndiceName);
            else if (indices.Length == 0)
                return Indices.Index(RelationName);
            else
                return Indices.Index(indices);
        }

        /// <summary>
        /// 获取索引
        /// </summary>
        /// <param name="indices">指定索引(null:默认，[]:全部(拥有相同别名的索引),["i_0","i_1","i_2"]:指定索引)</param>
        /// <returns></returns>
        private string GetIndex(string[] indices)
        {
            if (indices == null)
                return IndiceName;
            else if (indices.Length == 0)
                return RelationName;
            else
                return string.Join(",", indices);
        }

        /// <summary>
        /// 秒转换为时间
        /// </summary>
        /// <param name="time">时间(单位:秒)</param>
        /// <returns></returns>
        private string ConvertTime(int time)
        {
            if (time == 0)
                return "2m";
            else if (time >= 3600)
                return (time / 3600) + (time % 3600 > 0 ? 1 : 0) + "h";
            else if (time >= 60)
                return (time / 60) + (time % 60 > 0 ? 1 : 0) + "m";
            else
                return time + "s";
        }

        /// <summary>
        /// 获取查询
        /// </summary>
        /// <param name="indices"></param>
        /// <param name="query"></param>
        /// <param name="sort"></param>
        /// <param name="Transfinite"></param>
        /// <param name="time"></param>
        /// <param name="Source"></param>
        /// <returns></returns>
        private ISearchResponse<T> GetSearch<T>(
            string[] indices,
            Func<QueryContainerDescriptor<T>, QueryContainer> query,
            Dictionary<string, string> sort,
            bool Transfinite,
            string time,
            bool Source) where T : class
        {
            var indices_ = GetIndices(indices);

            return ElasticClient.Search<T>(s =>
            {
                if (Transfinite)
                    s.Size(10000);
                if (!Source)
                    s.Source(false);

                s = s.Index(indices_);

                if (query != null)
                    s = s.Query(query);
                else
                    s = s.MatchAll();
                if (sort != null && sort.Count > 0)
                    s = s.Sort(so =>
                    {
                        sort.ForEach(item =>
                        {
                            so = so.Field(typeof(T).GetProperty(item.Key), item.Value == "asc" ? SortOrder.Ascending : SortOrder.Descending);
                        });
                        return so;
                    });
                if (Transfinite)
                    s = s.Scroll(time);
                return s;
            });
        }
    }
}
