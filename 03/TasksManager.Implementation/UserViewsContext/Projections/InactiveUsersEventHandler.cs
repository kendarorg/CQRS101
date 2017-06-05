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
    public class InactiveUsersEventHandler :
        MockRepository<InactiveUser, Guid>,
        IEventHandler<UserModified>,
        IEventHandler<UserCreated>
    {
        public void Handle(UserCreated message)
        {
            var newUser = new InactiveUser
            {
                CompanyId = message.CompanyId,
                CompanyName = message.CompanyName,
                Id = message.Id,
                Password = message.Password,
                UserName = message.UserName
            };
            Save(newUser);
        }

        public void Handle(UserModified message)
        {
            var existing = GetById(message.Id);
            if (existing == null && message.IsEnabled)
            {
                var newUser = new InactiveUser
                {
                    CompanyId = message.CompanyId,
                    CompanyName = message.CompanyName,
                    Id = message.Id,
                    Password = message.Password,
                    UserName = message.UserName
                };
                Save(newUser);
            }
            else if (message.IsEnabled)
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

        #region Repository

        public override Guid GetKey(InactiveUser entity)
        {
            return entity.Id;
        }

        private static ConcurrentDictionary<string, InactiveUser> Storage;

        static InactiveUsersEventHandler()
        {
            Storage = new ConcurrentDictionary<string, InactiveUser>(StringComparer.InvariantCultureIgnoreCase);
        }

        public override void SetKey(InactiveUser entity, Guid key)
        {
            entity.Id = key;
        }

        protected override ConcurrentDictionary<string, InactiveUser> GetStorage()
        {
            return Storage;
        }

        protected override Guid InitializeKey(Guid key)
        {
            if (key == Guid.Empty) return Guid.NewGuid();
            return key;
        }
        #endregion Repository
    }
}
