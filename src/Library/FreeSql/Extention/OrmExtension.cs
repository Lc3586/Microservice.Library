using FreeSql;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Library.FreeSql.Extention
{
    public static class OrmExtension
    {
        /// <summary>
        /// 运行事务
        /// </summary>
        /// <param name="orm"></param>
        /// <param name="handler">事务体</param>
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
    }
}
