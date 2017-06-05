using Commons.Implementation;
using Cqrs.Commons;
using System;
using TasksManager.SharedContext.Events;
using TasksManager.UserViewsContext.Projections.Entities;
using System.Collections.Concurrent;

namespace TasksManager.Implementation.UserViewsContext.Projections
{
    public class ActiveCompaniesEventHandler :
        MockRepository<ActiveCompany, Guid>,
        IEventHandler<CompanyModified>
    {

        public void Handle(CompanyModified message)
        {
            var existing = GetById(message.Id);
            if (existing == null && message.IsEnabled)
            {
                var newCompany = new ActiveCompany
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

        public override Guid GetKey(ActiveCompany entity)
        {
            return entity.Id;
        }

        private static ConcurrentDictionary<string, ActiveCompany> Storage;

        static ActiveCompaniesEventHandler()
        {
            Storage = new ConcurrentDictionary<string, ActiveCompany>(StringComparer.InvariantCultureIgnoreCase);
        }

        public override void SetKey(ActiveCompany entity, Guid key)
        {
            entity.Id = key;
        }

        protected override ConcurrentDictionary<string, ActiveCompany> GetStorage()
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
