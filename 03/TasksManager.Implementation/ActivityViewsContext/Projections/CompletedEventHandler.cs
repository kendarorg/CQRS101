using Commons.Implementation;
using Cqrs.Commons;
using System;
using TasksManager.SharedContext.Events;
using TasksManager.ViewsContext.Projections.Entities;
using System.Collections.Concurrent;
using TasksManager.SharedContext.VOs;

namespace TasksManager.Implementation.ViewsContext.Projections
{
    public class CompletedEventHandler :
        MockRepository<CompletedActivity, Guid>,
        IEventHandler<ActivityCompleted>
    {
        private IActivityTypesService _typesService;

        public CompletedEventHandler(IActivityTypesService typesService)
        {
            _typesService = typesService;
        }
        public void Handle(ActivityCompleted message)
        {
            var type = _typesService.GetByCode(message.TypeCode);
            Storage[message.Id.ToString()] = new CompletedActivity
            {
                CompanyId = message.CompanyId,
                UserId = message.UserId,
                Day = message.Day,
                Description = message.Description,
                From = message.From,
                To = message.To,
                Id = message.Id,
                TypeCode = type.Code,
                TypeName = type.Name
            };
        }

        #region Repository

        public static ConcurrentDictionary<string, CompletedActivity> Storage { get; private set; }

        static CompletedEventHandler()
        {
            Storage = new ConcurrentDictionary<string, CompletedActivity>(
                StringComparer.InvariantCultureIgnoreCase);
        }

        public override Guid GetKey(CompletedActivity entity)
        {
            return entity.Id;
        }

        public override void SetKey(CompletedActivity entity, Guid key)
        {
            entity.Id = key;
        }

        protected override ConcurrentDictionary<string, CompletedActivity> GetStorage()
        {
            return Storage;
        }

        protected override Guid InitializeKey(Guid key)
        {
            if (key == Guid.Empty) return Guid.NewGuid();
            return key;
        }

        #endregion
    }
}