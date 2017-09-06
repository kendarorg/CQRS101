using Commons.Implementation;
using Cqrs.Commons;
using System;
using TasksManager.SharedContext.Events;
using TasksManager.UserViewsContext.Projections.Entities;
using System.Collections.Concurrent;

namespace TasksManager.Implementation.UserViewsContext.Projections
{
    public class InactiveCompaniesEventHandler :
        MockRepository<InactiveCompany, Guid>,
        IEventHandler<CompanyModified>,
        IEventHandler<CompanyCreated>
    {
        public void Handle(CompanyCreated message)
        {
            var newCompany = new InactiveCompany
            {
                Name = message.CompanyName,
                Id = message.Id
            };
            Save(newCompany);
        }

        public void Handle(CompanyModified message)
        {
            var existing = GetById(message.Id);
            if (existing == null && message.IsEnabled)
            {
                var newCompany = new InactiveCompany
                {
                    Name = message.CompanyName,
                    Id = message.Id
                };
                Save(newCompany);
            }
            else if (!message.IsEnabled)
            {
                Delete(message.Id);
            }
            else
            {
                existing.Id = message.Id;
                existing.Name = message.CompanyName;
                Update(existing);
            }
        }

        #region Repository

        public override Guid GetKey(InactiveCompany entity)
        {
            return entity.Id;
        }

        private static ConcurrentDictionary<string, InactiveCompany> Storage;

        static InactiveCompaniesEventHandler()
        {
            Storage = new ConcurrentDictionary<string, InactiveCompany>(StringComparer.InvariantCultureIgnoreCase);
        }

        public override void SetKey(InactiveCompany entity, Guid key)
        {
            entity.Id = key;
        }

        protected override ConcurrentDictionary<string, InactiveCompany> GetStorage()
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
