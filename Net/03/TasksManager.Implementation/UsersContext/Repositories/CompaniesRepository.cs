using Commons;
using Commons.Implementation;
using System;
using TasksManager.Implementation.UsersContext.Repositories.Entities;
using System.Collections.Concurrent;

namespace TasksManager.Implementation.UsersContext.Repositories
{
    public class CompaniesRepository : MockRepository<Company, Guid>
    {
        private static readonly ConcurrentDictionary<string, Company> Storage;

        static CompaniesRepository()
        {
            Storage = new ConcurrentDictionary<string, Company>();
        }

        public override Guid GetKey(Company entity)
        {
            return entity.Id;
        }

        public override void SetKey(Company entity, Guid key)
        {
            entity.Id = key;
        }

        protected override ConcurrentDictionary<string, Company> GetStorage()
        {
            return Storage;
        }

        protected override Guid InitializeKey(Guid key)
        {
            if (key == Guid.Empty) return Guid.NewGuid();
            return key;
        }
    }
}
