using FreeSql;
using Library.FreeSql.Gen;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.FreeSql.Extention
{
    public static class CodeFirstExtention
    {
        /// <summary>
        /// 同步实体
        /// </summary>
        /// <returns></returns>
        public static void SyncStructure(this ICodeFirst codeFirst, params Type[] types)
        {
            if (!codeFirst.SyncStructure(types))
                throw new FreeSqlError("同步实体类型集合到数据库失败");
        }
    }
}
