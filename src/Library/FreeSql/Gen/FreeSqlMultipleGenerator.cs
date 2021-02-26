using FreeSql;
using FreeSql.Internal;
using Microservice.Library.FreeSql.Application;
using Microservice.Library.FreeSql.Extention;
using Microservice.Library.FreeSql.Repository;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Microservice.Library.FreeSql.Gen
{
    /// <summary>
    /// 多库生成器
    /// </summary>
    /// <typeparam name="TKey">库标识类型</typeparam>
    public class FreeSqlMultipleGenerator<TKey> : IFreeSqlMultipleProvider<TKey>
    {
        private readonly FreeSqlMultipleGenOptions<TKey> Options;

        public FreeSqlMultipleGenerator(FreeSqlMultipleGenOptions<TKey> options)
        {
            Options = options ?? new FreeSqlMultipleGenOptions<TKey>();

            if (Options.UseIdleBus)
            {
                IdleBus = new IdleBus<TKey, IFreeSql>(Options.Idle);
                IdleBus.Notice += Notice;
            }
            else
            {
                Dic = new Dictionary<TKey, IFreeSql>();
                getTimes = new Dictionary<TKey, long>();
            }

            //Register();
        }

        protected IdleBus<TKey, IFreeSql> IdleBus;

        protected Dictionary<TKey, IFreeSql> Dic;

        /// <summary>
        /// 实例获取次数
        /// </summary>
        private Dictionary<TKey, long> getTimes;

        /// <summary>
        /// 实例获取次数
        /// </summary>
        private string GetTimes(TKey key)
        {
            if (!getTimes.ContainsKey(key))
                getTimes.Add(key, 0);

            if (getTimes[key] < long.MaxValue)
                getTimes[key]++;
            else
                getTimes[key] = -1;

            return getTimes[key] == -1 ? $"大于 {long.MaxValue}" : getTimes[key].ToString();
        }

        /// <summary>
        /// 接收通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Notice(object sender, IdleBus<TKey, IFreeSql>.NoticeEventArgs e)
        {
            switch (e.NoticeType)
            {
                case IdleBus<TKey, IFreeSql>.NoticeType.Register:
                    break;
                case IdleBus<TKey, IFreeSql>.NoticeType.Remove:
                    break;
                case IdleBus<TKey, IFreeSql>.NoticeType.AutoCreate:
                    break;
                case IdleBus<TKey, IFreeSql>.NoticeType.AutoRelease:
                    break;
                case IdleBus<TKey, IFreeSql>.NoticeType.Get:
                    break;
                default:
                    break;
            }

            try
            {
                Options.Notice?.Invoke(e);
            }
            catch (Exception ex)
            {
                throw new FreeSqlException("执行配置的通知处理方法时发生错误", ex);
            }
        }

        ///// <summary>
        ///// 注册全部库
        ///// </summary>
        //private void Register()
        //{
        //    foreach (var item in Options.KeyOptionsCollection.Keys)
        //    {
        //        Register(item);
        //    }
        //}

        /// <summary>
        /// 注册库
        /// </summary>
        /// <param name="key">库标识</param>
        private void Register(TKey key)
        {
            Func<IFreeSql> getOrm = () => new FreeSqlGenerator(Options.KeyOptionsCollection[key]).GetFreeSql();

            if (Options.UseIdleBus)
                IdleBus.Register(key, getOrm);
            else
            {
                var watch = new Stopwatch();
                Notice(null, new IdleBus<TKey, IFreeSql>.NoticeEventArgs(
                    IdleBus<TKey, IFreeSql>.NoticeType.Register,
                    key,
                    null,
                    $"{key} 注册成功, 0/1."));

                try
                {
                    watch.Start();
                    Dic.Add(key, getOrm.Invoke());
                    watch.Stop();
                    Notice(null, new IdleBus<TKey, IFreeSql>.NoticeEventArgs(
                        IdleBus<TKey, IFreeSql>.NoticeType.AutoCreate,
                        key,
                        null,
                        $"{key} 实例+++创建成功, 耗时 {watch.ElapsedMilliseconds: 00}ms, 1/1."));
                }
                catch (Exception ex)
                {
                    Notice(null, new IdleBus<TKey, IFreeSql>.NoticeEventArgs(
                        IdleBus<TKey, IFreeSql>.NoticeType.Register,
                        key,
                        ex,
                        $"{key} 注册失败."));

                    throw new FreeSqlException($"{key} 注册失败.", ex);
                }
            }
        }

        /// <summary>
        /// 检查库
        /// </summary>
        /// <param name="key">库标识</param>
        private void Check(TKey key)
        {
            if (!Options.KeyOptionsCollection.ContainsKey(key))
            {
                var ex = new FreeSqlException($"库 {key} 不存在.");

                if (!Options.UseIdleBus)
                    Notice(null, new IdleBus<TKey, IFreeSql>.NoticeEventArgs(
                        IdleBus<TKey, IFreeSql>.NoticeType.Get,
                        key,
                        ex,
                        $"{key} 实例获取失败."));

                throw ex;
            }

            if (Options.UseIdleBus ? !IdleBus.Exists(key) : !Dic.ContainsKey(key))
                Register(key);
        }

        /// <summary>
        /// 获取库
        /// </summary>
        /// <param name="key">库标识</param>
        /// <returns></returns>
        private IFreeSql Get(TKey key)
        {
            var watch = new Stopwatch();
            watch.Start();

            Check(key);

            watch.Stop();

            if (Options.UseIdleBus)
                return IdleBus.Get(key);
            else
            {
                Notice(null, new IdleBus<TKey, IFreeSql>.NoticeEventArgs(
                    IdleBus<TKey, IFreeSql>.NoticeType.Get,
                    key,
                    null,
                    $"{key} 实例获取成功 {GetTimes(key)}次{(watch.ElapsedMilliseconds == 0 ? "" : $", 耗时 { watch.ElapsedMilliseconds: 00}ms")}."));
                return Dic[key];
            }
        }

        /// <summary>
        /// 同步库结构
        /// </summary>
        /// <param name="key">库标识</param>
        /// <param name="orm">orm</param>
        private void SyncStructure(TKey key, IFreeSql orm)
        {
            var options = Options.KeyOptionsCollection[key];
            if (options.FreeSqlDevOptions?.AutoSyncStructure == true && options.FreeSqlDevOptions?.SyncStructureOnStartup == true)
                orm.CodeFirst.SyncStructure(new EntityFactory(options.FreeSqlDbContextOptions).GetEntitys(options.FreeSqlDbContextOptions.EntityKey).ToArray());
        }

        public bool Exists(TKey key)
        {
            return Options.KeyOptionsCollection.ContainsKey(key);
        }

        public bool ExistsAndRegistered(TKey key)
        {
            if (!Exists(key))
                return false;

            return Options.UseIdleBus ? IdleBus.Exists(key) : Dic.ContainsKey(key);
        }

        public IFreeSql GetFreeSql(TKey key)
        {
            var orm = Get(key);

            SyncStructure(key, orm);

            return orm;
        }

        public List<IFreeSql> GetAllFreeSql()
        {
            return GetAllFreeSqlWithKey().Select(o => o.Value).ToList();
        }

        public Dictionary<TKey, IFreeSql> GetAllFreeSqlWithKey()
        {
            var orms = new Dictionary<TKey, IFreeSql>();
            foreach (var key in Options.KeyOptionsCollection.Keys)
            {
                orms.Add(key, GetFreeSql(key));
            }
            return orms;
        }

        public void SyncStructure(TKey key)
        {
            SyncStructure(key, Get(key));
        }

        public void SyncAllStructure()
        {
            foreach (var key in Options.KeyOptionsCollection.Keys)
            {
                SyncStructure(key, Get(key));
            }
        }

        public BaseDbContext GetDbContext(TKey key)
        {
            var db = new BaseDbContext(GetFreeSql(key), Options.KeyOptionsCollection[key].FreeSqlDbContextOptions);
            return db;
        }

        public List<BaseDbContext> GetAllDbContext()
        {
            return GetAllDbContextWithKey().Select(o => o.Value).ToList();
        }

        public Dictionary<TKey, BaseDbContext> GetAllDbContextWithKey()
        {
            var dbs = new Dictionary<TKey, BaseDbContext>();
            foreach (var key in Options.KeyOptionsCollection.Keys)
            {
                dbs.Add(key, GetDbContext(key));
            }
            return dbs;
        }
    }
}
