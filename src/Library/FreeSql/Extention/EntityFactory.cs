using FreeSql.DataAnnotations;
using JetBrains.Annotations;
using Library.FreeSql.Application;
using Library.FreeSql.Gen;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.FreeSql.Extention
{
    public class EntityFactory
    {
        public EntityFactory()
        {

        }

        public EntityFactory([NotNull] FreeSqlDbContextOptions freeSqlDbContextOptions)
        {
            _freeSqlDbContextOptions = freeSqlDbContextOptions;
        }

        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <param name="key">标识</param>
        /// <returns></returns>
        public IEnumerable<Type> GetEntitys(string key)
        {
            bool success = _dbCompiledModel.TryGetValue(key, out IEnumerable<Type> resModel);
            if (!success)
            {
                var theLock = _lockDic.GetOrAdd(key, new object());
                lock (theLock)
                {
                    success = _dbCompiledModel.TryGetValue(key, out resModel);
                    if (!success)
                    {
                        resModel = InitEntityType();
                        _dbCompiledModel[key] = resModel;
                    }
                }
            }

            return resModel;
        }

        private IEnumerable<Type> InitEntityType()
        {
            if (_freeSqlDbContextOptions == null)
                throw new FreeSqlException("FreeSqlDbContextOptions不能为空");
            var assembly = _freeSqlDbContextOptions.EntityAssembly.Select(o => Assembly.Load(o));
            if (assembly == null)
                throw new FreeSqlException($"命名空间{_freeSqlDbContextOptions.EntityAssembly}不存在");
            return assembly.SelectMany(o => o.GetTypes())
                .Where(x => x.GetCustomAttribute(typeof(TableAttribute), false) != null);
        }

        FreeSqlDbContextOptions _freeSqlDbContextOptions { get; set; }

        private static ConcurrentDictionary<string, IEnumerable<Type>> _dbCompiledModel { get; } = new ConcurrentDictionary<string, IEnumerable<Type>>();

        private static readonly ConcurrentDictionary<string, object> _lockDic
            = new ConcurrentDictionary<string, object>();
    }
}
