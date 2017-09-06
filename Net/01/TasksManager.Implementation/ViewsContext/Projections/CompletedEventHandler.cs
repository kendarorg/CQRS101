using Commons.Implementation;
using Cqrs.Commons;
using System;
using TasksManager.SharedContext.Events;
using TasksManager.ViewsContext.Projections.Entities;
using System.Collections.Concurrent;

namespace TasksManager.Implementation.ViewsContext.Projections
{
    public class CompletedEventHandler :
        MockRepository<CompletedActivity, Guid>,
        IEventHandler<ActivityCompleted>
    {
        public void Handle(ActivityCompleted message)
        {
            Storage[message.Id.ToString()] = new CompletedActivity
            {
                Day = message.Day,
                Description = message.Description,
                From = message.From,
                To = message.To,
                Id = message.Id
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