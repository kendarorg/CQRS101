using Commons.Implementation;
using Cqrs.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManager.SharedContext.Events;
using TasksManager.UserViewsContext.Projections.Entities;
using System.Collections.Concurrent;

namespace TasksManager.Implementation.UserViewsContext.Projections
{
    public class ActiveUsersEventHandler :
        MockRepository<ActiveUser, Guid>,
        IEventHandler<UserModified>
    {

        public void Handle(UserModified message)
        {
            var existing = GetById(message.Id);
            if (existing == null && message.IsEnabled)
            {
                var newUser = new ActiveUser
                {
                    CompanyId = message.CompanyId,
                    CompanyName = message.CompanyName,
                    Id = message.Id,
                    Password = message.Password,
                    UserName = message.UserName
                };
                Save(newUser);
            }
            else if (!message.IsEnabled)
            {
                Delete(message.Id);
            }
            else
            {
                existing.CompanyId = message.CompanyId;
                existing.CompanyName = message.CompanyName;
                existing.Password = message.Password;
                existing.UserName = message.UserName;
                Update(existing);
            }
        }

        public override Guid GetKey(ActiveUser entity)
        {
            return entity.Id;
        }

        private static ConcurrentDictionary<string, ActiveUser> Storage;

        static ActiveUsersEventHandler()
        {
            Storage = new ConcurrentDictionary<string, ActiveUser>(StringComparer.InvariantCultureIgnoreCase);
        }

        public override void SetKey(ActiveUser entity, Guid key)
        {
            entity.Id = key;
        }

        protected override ConcurrentDictionary<string, ActiveUser> GetStorage()
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
