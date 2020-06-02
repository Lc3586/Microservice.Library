using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace Library.TimeTick
{
    /// <summary>
    /// 定时器
    /// </summary>
    public class Interval
    {
        static Interval()
        {
            Cache = new ServiceCollection()
                    .AddMemoryCache()
                    .BuildServiceProvider()
                    .GetService<IMemoryCache>();
        }

        /// <summary>
        /// 默认缓存
        /// </summary>
        public static IMemoryCache Cache { get; set; }

        /// <summary>
        /// 设置一个时间间隔的循环操作
        /// </summary>
        /// <param name="action">执行的操作</param>
        /// <param name="timeSpan">时间间隔</param>
        public static Timer SetInterval(Action action, TimeSpan timeSpan)
        {
            var key = Guid.NewGuid().ToString();
            Timer threadTimer = new Timer((state =>
            {
                var timer = Cache.Get<Timer>(key);
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                action.Invoke();
                timer.Change((long)timeSpan.TotalMilliseconds, (long)timeSpan.TotalMilliseconds);
            }), null, 0, (long)timeSpan.TotalMilliseconds);
            Cache.Set(key, threadTimer);

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
            var key = Guid.NewGuid().ToString();
            Timer threadTimer = new Timer((state =>
            {
                var timer = Cache.Get<Timer>(key);
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                action.Invoke();
                timer.Change((long)timeSpan.TotalMilliseconds, (long)timeSpan.TotalMilliseconds);
            }), null, dely, timeSpan);
            Cache.Set(key, threadTimer);

            return threadTimer;
        }
    }
}
