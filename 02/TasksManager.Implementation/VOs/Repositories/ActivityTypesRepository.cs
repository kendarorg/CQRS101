using System;
using System.Collections.Concurrent;
using Commons.Implementation;
using TasksManager.VOs.Entities;

namespace TasksManager.Implementation.VOs.Repositories
{
    public class ActivityTypesRepository :
        MockRepository<ActivityType, string>
    {
        public static ConcurrentDictionary<string, ActivityType> Storage { get; private set; }

        static ActivityTypesRepository()
        {
            Storage = new ConcurrentDictionary<string, ActivityType>(
                StringComparer.InvariantCultureIgnoreCase);
        }

        public override string GetKey(ActivityType entity)
        {
            return entity.Code;
        }

        public override void SetKey(ActivityType entity, string key)
        {
            entity.Code = key;
        }

        protected override ConcurrentDictionary<string, ActivityType> GetStorage()
        {
            return Storage;
        }

        protected override string InitializeKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new NotSupportedException();
            return key;
        }
    }
}
