using System;
using System.Collections.Generic;
using System.Threading;

namespace Library.TimerTick
{
    /// <summary>
    /// 定时器
    /// LCTR 2019-08-27
    /// </summary>
    public class TimeTick
    {
        public TimeTick()
        {

        }

        public TimeTick(TimeSpan dely, TimeSpan timeSpan)
        {
            _dely = dely;
            _timeSpan = timeSpan;
        }

        private TimeSpan _dely;

        private TimeSpan _timeSpan;

        private Dictionary<object, Timer> timeTick = new Dictionary<object, Timer>();

        public void Start(object key)
        {
            Start(key, null, null, _dely, _timeSpan);
        }

        public void Start(object key, TimeSpan dely, TimeSpan timeSpan)
        {
            Start(key, null, null, dely, timeSpan);
        }

        public void Start(object key, Action<object> task, object state)
        {
            if (timeTick.ContainsKey(key))
                timeTick[key].Change(_dely, _timeSpan);
            else
                timeTick.Add(key, new Timer(new TimerCallback(task), state, _dely, _timeSpan));
        }

        public void Start(object key, Action<object> task, object state, TimeSpan dely, TimeSpan timeSpan)
        {
            if (timeTick.ContainsKey(key))
                timeTick[key].Change(dely, timeSpan);
            else
                timeTick.Add(key, new Timer(new TimerCallback(task), state, dely, timeSpan));
        }

        public void Stop(object key)
        {
            if (timeTick.ContainsKey(key))
                timeTick[key].Change(-1, 1);
        }
    }
}
