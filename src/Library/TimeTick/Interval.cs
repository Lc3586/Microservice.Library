using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace Microservice.Library.TimeTick
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

    }
}
