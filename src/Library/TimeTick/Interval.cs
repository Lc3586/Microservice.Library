using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;

namespace Library.TimeTick
{
    /// <summary>
    /// 定时器
    /// </summary>
    public class Interval
    {
        /// <summary>
        /// 默认缓存
        /// </summary>
        public static IMemoryCache Cache { get; }

        /// <summary>
        /// 设置一个时间间隔的循环操作
        /// </summary>
        /// <param name="action">执行的操作</param>
        /// <param name="timeSpan">时间间隔</param>
        public static Timer SetInterval(Action action, TimeSpan timeSpan)
        {
            //var provider = new ServiceCollection()
            //           .AddMemoryCache()
            //           .BuildServiceProvider();

            ////And now?
            //var cache = provider.GetService<IMemoryCache>();
            Timer threadTimer = new Timer((state =>
            {
                action.Invoke();
            }), null, 0, (long)timeSpan.TotalMilliseconds);
            Cache.Set(Guid.NewGuid().ToString(), threadTimer);

            return threadTimer;
        }

        /// <summary>
        /// 设置一个时间间隔的循环操作
        /// </summary>
        /// <param name="action">执行的操作</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="dely">延迟一段时间后再开始循环</param>
        public static Timer SetInterval(Action action, TimeSpan timeSpan, TimeSpan dely)
        {
            Timer threadTimer = new Timer((state =>
            {
                action.Invoke();
            }), null, dely, timeSpan);
            Cache.Set(Guid.NewGuid().ToString(), threadTimer);

            return threadTimer;
        }
    }
}
