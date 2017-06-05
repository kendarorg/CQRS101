using Commons.Implementation;
using Cqrs.Commons;
using System;
using System.Collections.Concurrent;
using TasksManager.SharedContext.Events;
using TasksManager.SharedContext.VOs;
using TasksManager.ViewsContext.Projections.Entities;

namespace TasksManager.Implementation.ViewsContext.Projections
{
    public class ActivitySummaryEventHandler :
        MockRepository<ActivitySummary, ActivitySummaryKey>,
        IEventHandler<ActivityCompleted>
    {
        private IActivityTypesService _typesService;

        public ActivitySummaryEventHandler(IActivityTypesService typesService)
        {
            _typesService = typesService;
        }

        public void Handle(ActivityCompleted message)
        {
            var type = _typesService.GetByCode(message.TypeCode);
            var key = new ActivitySummaryKey
            {
                Day = message.Day,
                TypeCode = message.TypeCode
            };
            var oldSummary = GetById(key);
            var summary = new ActivitySummary
            {
                Day = message.Day,
                Description = message.Description,
                Duration = message.To - message.From,
                TypeCode = type.Code,
                TypeName = type.Name
            };
            if (oldSummary != null)
            {
                oldSummary.Duration += summary.Duration;
                summary = oldSummary;
            }
            Save(summary);
        }

        #region Repository

        public static ConcurrentDictionary<string, ActivitySummary> Storage { get; private set; }

        static ActivitySummaryEventHandler()
        {
            Storage = new ConcurrentDictionary<string, ActivitySummary>(
                StringComparer.InvariantCultureIgnoreCase);
        }

        public override ActivitySummaryKey GetKey(ActivitySummary entity)
        {
            return new ActivitySummaryKey
            {
                Day = entity.Day,
                TypeCode = entity.TypeCode
            };
        }

        public override void SetKey(ActivitySummary entity, ActivitySummaryKey key)
        {
            entity.Day = key.Day;
            entity.TypeCode = key.TypeCode;
        }

        protected override ConcurrentDictionary<string, ActivitySummary> GetStorage()
        {
            return Storage;
        }

        protected override ActivitySummaryKey InitializeKey(ActivitySummaryKey key)
        {
            return key;
        }

        #endregion
    }
}
