using Commons;
using Commons.Implementation;
using Cqrs.Commons;
using System;
using TasksManager.SharedContext.Events;
using TasksManager.ViewsContext.Projections.Entities;
using System.Collections.Concurrent;

namespace TasksManager.Implementation.ViewsContext.Projections
{
    public class NotCompletedEventHandler :
        MockRepository<NotCompletedActivity, Guid>,
        IEventHandler<ActivityCreated>,
        IEventHandler<ActivityCompleted>
    {
        public void Handle(ActivityCompleted message)
        {
            if (GetById(message.Id) != null)
            {
                Delete(message.Id);
            }
        }

        public void Handle(ActivityCreated message)
        {
            if (GetById(message.Id) == null)
            {
                Save(new NotCompletedActivity
                {
                    Day = message.Day,
                    Description = message.Description,
                    From = message.From,
                    Id = message.Id
                });
            }
        }

        #region Repository

        public static ConcurrentDictionary<string, NotCompletedActivity> Storage { get; private set; }

        static NotCompletedEventHandler()
        {
            Storage = new ConcurrentDictionary<string, NotCompletedActivity>(
                StringComparer.InvariantCultureIgnoreCase);
        }

        public override Guid GetKey(NotCompletedActivity entity)
        {
            return entity.Id;
        }

        public override void SetKey(NotCompletedActivity entity, Guid key)
        {
            entity.Id = key;
        }

        protected override ConcurrentDictionary<string, NotCompletedActivity> GetStorage()
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