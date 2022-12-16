using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Library.Extension
{
    /// <summary>
    /// 拓展类
    /// </summary>
    public static partial class Extension
    {
        /// <summary>
        /// 循环指定次数
        /// </summary>
        /// <param name="count">循环次数</param>
        /// <param name="method">执行的方法</param>
        public static void Loop(this Action method, int count)
        {
            for (int i = 0; i < count; i++)
            {
                method();
            }
        }

        /// <summary>
        /// 循环指定次数
        /// </summary>
        /// <param name="count">循环次数</param>
        /// <param name="method">执行的方法</param>
        public static void Loop(this Action<int> method, int count)
        {
            for (int i = 0; i < count; i++)
            {
                method(i);
            }
        }

        /// <summary>
        /// 异步执行方法
        /// </summary>
        /// <param name="firstFunc">首先执行的方法</param>
        /// <param name="next">接下来执行的方法</param>
        public static void RunAsync(this Action firstFunc, Action next)
        {
            Task firstTask = new Task(() =>
            {
                firstFunc();
            });

            firstTask.Start();
            firstTask.ContinueWith(x => next());
        }

        /// <summary>
        /// 异步执行方法
        /// </summary>
        /// <param name="firstFunc">首先执行的方法</param>
        /// <param name="next">接下来执行的方法</param>
        public static void RunAsync(this Func<object> firstFunc, Action<object> next)
        {
            Task<object> firstTask = new Task<object>(() =>
            {
                return firstFunc();
            });

            firstTask.Start();
            firstTask.ContinueWith(x => next(x.Result));
        }

        /// <summary>
        /// 设置一个时间间隔的循环操作
        /// </summary>
        /// <param name="action">执行的操作</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="cancellationToken">请求取消循环</param>
        public static void SetInterval(this Action action, TimeSpan timeSpan, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                action.Invoke();
                Task.Delay(timeSpan).GetAwaiter().GetResult();
            }
        }

        /// <summary>
        /// 设置一个时间间隔的循环操作
        /// </summary>
        /// <param name="action">执行的操作</param>
        /// <param name="timeSpan">时间间隔</param>
        /// <param name="dely">延迟一段时间后再开始循环</param>
        /// <param name="cancellationToken">请求取消循环</param>
        public static void SetInterval(this Action action, TimeSpan timeSpan, TimeSpan dely, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Task.Delay(dely).GetAwaiter().GetResult();
                action.Invoke();
                Task.Delay(timeSpan).GetAwaiter().GetResult();
            }
        }
    }
}
