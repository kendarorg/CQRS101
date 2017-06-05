using System;
using System.Collections.Concurrent;
using Commons.Implementation;
using TasksManager.Implementation.ActivityContext.Repositories.Entities;

namespace Cqrs.ActivityDayContext.Repositories
{
    public class ActivityDayRepository : MockRepository<ActivityDay, int>
    {
        #region Repository

        private static readonly ConcurrentDictionary<string, ActivityDay> Storage;

        static ActivityDayRepository()
        {
            Storage = new ConcurrentDictionary<string, ActivityDay>(StringComparer.InvariantCultureIgnoreCase);
        }

        protected override ConcurrentDictionary<string, ActivityDay> GetStorage()
        {
            return Storage;
        }

        public override int GetKey(ActivityDay entity)
        {
            return entity.Day;
        }

        public override void SetKey(ActivityDay entity, int key)
        {
            entity.Day = key;
        }

        protected override int InitializeKey(int key)
        {
            if (key <= 0) return DateTime.Now.Day +
                    DateTime.Now.Month * 100 +
                    DateTime.Now.Year * 100 * 100;
            return key;
        }

        #endregion
    }
}
