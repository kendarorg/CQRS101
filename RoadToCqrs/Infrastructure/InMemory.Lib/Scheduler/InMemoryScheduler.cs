using Scheduler;
using System;
using System.Collections.Generic;

namespace InMemory.Scheduler
{
    public class InMemoryScheduler : IScheduler
    {
        private static readonly List<Action<DateTime>> Actions = new List<Action<DateTime>>();
        public void Register(TimeSpan span, Action<DateTime> action)
        {
            Actions.Add(action);
        }

        // ReSharper disable once MemberCanBeMadeStatic.Global
        public void ForceRun(DateTime now)
        {
            foreach(var action in Actions)
            {
                try
                {
                    action(now);
                }
                catch (Exception)
                {
                    //NOP
                }
            }
        }
    }
}
