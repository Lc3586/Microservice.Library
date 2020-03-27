using FreeSql;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.FreeSql.Extention
{
    public static class RepositoryExtention
    {
        public static TEntity GetAndCheckNull<TEntity, TKey>(this BaseRepository<TEntity, TKey> repository, TKey id, string error = "数据不存在或已失效") where TEntity : class
        {
            var data = repository.Get(id);
            if (data == null)
                throw new MessageException(error);
            return data;
        }
    }
}
