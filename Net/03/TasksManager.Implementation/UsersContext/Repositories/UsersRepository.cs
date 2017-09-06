using Commons;
using Commons.Implementation;
using System;
using TasksManager.Implementation.UsersContext.Repositories.Entities;
using System.Collections.Concurrent;

namespace TasksManager.Implementation.UsersContext.Repositories
{
    public class UsersRepository : MockRepository<User, Guid>
    {
        private static readonly ConcurrentDictionary<string, User> Storage;

        static UsersRepository()
        {
            Storage = new ConcurrentDictionary<string, User>();
        }

        public override Guid GetKey(User entity)
        {
            return entity.Id;
        }

        public override void SetKey(User entity, Guid key)
        {
            entity.Id = key;
        }

        protected override ConcurrentDictionary<string, User> GetStorage()
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
