using System;
using System.Collections.Concurrent;
using Commons.Implementation;
using Cqrs.VoContext.Repositories.Entities;

namespace Cqrs.VoContext.Repositories
{
    public class ActivityTypesRepository : MockRepository<ActivityType, string>
    {
        static ActivityTypesRepository()
        {
            Storage = new ConcurrentDictionary<string, ActivityType>(StringComparer.InvariantCultureIgnoreCase);
        }

        #region Repository

        private static readonly ConcurrentDictionary<string, ActivityType> Storage;

        protected override ConcurrentDictionary<string, ActivityType> GetStorage()
        {
            return Storage;
        }

        public override string GetKey(ActivityType entity)
        {
            return entity.Code;
        }

        public override void SetKey(ActivityType entity, string key)
        {
            entity.Code = key;
        }

        protected override string InitializeKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");
            return key;
        }

        #endregion Repository
    }
}
