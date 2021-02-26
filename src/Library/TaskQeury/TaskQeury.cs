using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Microservice.Library.TaskQeury
{
    /// <summary>
    /// 任务队列
    /// LCTR 2019-08-27
    /// </summary>
    public class TaskQeury
    {
        public TaskQeury()
        {

        }

        public TaskQeury(TimeSpan timeSpan)
        {
            TimeSpan = timeSpan;
        }

        private bool RunFlag { get; set; } = true;

        private TimeSpan TimeSpan { get; set; } = TimeSpan.Zero;

        Semaphore Semaphore { get; } = new Semaphore(0, int.MaxValue);

        private ConcurrentQueue<Action> TaskList { get; } = new ConcurrentQueue<Action>();

        private void Run()
        {
            while (RunFlag && TaskList.Count > 0)
            {
                Semaphore.WaitOne();
                if (TaskList.TryDequeue(out Action task))
                    task?.Invoke();

                if (TimeSpan != TimeSpan.Zero)
                    Thread.Sleep(TimeSpan);
            }
            RunFlag = false;
        }

        public void Start()
        {
            if (RunFlag)
                return;
            RunFlag = true;
            Run();
        }

        public void Stop()
        {
            RunFlag = false;
        }

        public void Enqueue(Action task)
        {
            lock (TaskList)
            {
                Stop();
                TaskList.Enqueue(task);
                Semaphore.Release();
                Start();
            }
        }

        public void Clear()
        {
            lock (TaskList)
            {
                while (TaskList.Count > 0)
                {
                    TaskList.TryDequeue(out Action task);
                }
            }

        }
    }
}
