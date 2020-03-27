using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Library.FreeSql.Extention
{
    public static class OrmExtention
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orm"></param>
        /// <param name="handler">事务体</param>
        /// <param name="timeout">超时设置</param>
        /// <param name="isolationLevel">事务隔离级别</param>
        /// <returns></returns>
        public static (bool Success, Exception Ex) RunTransaction(this IFreeSql orm, Action handler, TimeSpan? timeout = null, IsolationLevel? isolationLevel = null)
        {
            try
            {
                if (isolationLevel != null)
                    orm.Transaction(isolationLevel.Value, timeout.Value, handler);
                else if (timeout != null)
                    orm.Transaction(timeout.Value, handler);
                else
                    orm.Transaction(handler);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex);
            }
        }
    }
}
