using Library.FreeSql;
using System;
using System.Collections.Generic;

namespace Library.FreeSql.Application
{
    /// <summary>
    /// 多库生成配置
    /// </summary>
    public class FreeSqlMultipleGenOptions<TKey>
    {
        /// <summary>
        /// 使用IdleBus管理库对象
        /// <para>默认开启</para>
        /// <para>必须每次使用时都Get</para>
        /// <para>如果留着引用，对象空闲或超时会被回收，导致报错（对象已释放，无法访问）</para>
        /// </summary>
        internal bool UseIdleBus = true;

        /// <summary>
        /// 通知事件处理方法
        /// </summary>
        internal Action<IdleBus<TKey, IFreeSql>.NoticeEventArgs> Notice = (arg) => { };

        /// <summary>
        /// 键:库标识
        /// 值:生成配置
        /// </summary>
        internal Dictionary<TKey, FreeSqlGenOptions> KeyOptionsCollection { get; set; } = new Dictionary<TKey, FreeSqlGenOptions>();

        /// <summary>
        /// 禁用IdleBus管理库对象
        /// <para>默认启用</para>
        /// <para>启用时必须每次使用时都Get</para>
        /// <para>如果留着引用，库对象空闲或超时会被回收，导致报错（对象已释放，无法访问）</para>
        /// </summary>
        public FreeSqlMultipleGenOptions<TKey> DisableIdleBus()
        {
            UseIdleBus = false; return this;
        }

        /// <summary>
        /// 设置通知事件处理方法
        /// </summary>
        /// <param name="notice">通知事件处理方法</param>
        public FreeSqlMultipleGenOptions<TKey> SetUpNotice(Action<IdleBus<TKey, IFreeSql>.NoticeEventArgs> notice)
        {
            Notice = notice; return this;
        }

        /// <summary>
        /// 添加库配置
        /// </summary>
        /// <param name="key">库标识</param>
        /// <param name="setupAction">设置</param>
        /// <returns></returns>
        public FreeSqlMultipleGenOptions<TKey> Add(TKey key, Action<FreeSqlGenOptions> setupAction)
        {
            var options = new FreeSqlGenOptions();
            setupAction.Invoke(options);
            KeyOptionsCollection.Add(key, options);
            return this;
        }
    }
}
