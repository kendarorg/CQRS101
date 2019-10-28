using System;

namespace Scheduler
{
    public interface IScheduler
    {
        void Register(TimeSpan span, Action<DateTime> action);
    }
}
