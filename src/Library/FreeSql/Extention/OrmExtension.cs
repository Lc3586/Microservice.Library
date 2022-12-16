using FreeSql.Internal.Model;
using System;
using System.Data;

namespace Microservice.Library.FreeSql.Extention
{
    public static class OrmExtension
    {
        /// <summary>
        /// 运行事务
        /// </summary>
        /// <param name="orm"></param>
        /// <param name="handler"></param>
        /// <param name="isolationLevel">事务隔离级别</param>
        /// <returns></returns>
        public static (bool Success, Exception Ex) RunTransaction(this IFreeSql orm, Action handler, IsolationLevel? isolationLevel = null)
        {
            try
            {
                if (isolationLevel != null)
                    orm.Transaction(isolationLevel.Value, handler);
                else
                    orm.Transaction(handler);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex);
            }
        }

        /// <summary>
        /// 获取实体表信息
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="orm"></param>
        /// <returns></returns>
        public static TableInfo GetTableInfo<TSource>(this IFreeSql orm)
        {
            var type = typeof(TSource);
            TableInfo tableInfo;
            if (CacheExtention.TableInfoDic.ContainsKey(type))
                tableInfo = CacheExtention.TableInfoDic[type];
            else
            {
                tableInfo = orm.CodeFirst.GetTableByEntity(type);
                CacheExtention.TableInfoDic.Add(type, tableInfo);
            }

            return tableInfo;
        }
    }
}
