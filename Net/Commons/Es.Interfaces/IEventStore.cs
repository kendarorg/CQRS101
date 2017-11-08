using Cqrs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Es.Interfaces
{
    public interface IEventStore
    {
        List<IEvent> getEventsForAggregate(Guid aggregateId);
        void saveEvent(Guid aggregateId, List<IEvent> uncommittedChanges, long expectedVersion);
    }
}
