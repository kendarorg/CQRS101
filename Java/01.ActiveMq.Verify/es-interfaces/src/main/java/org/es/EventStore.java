package org.es;

import java.util.List;
import java.util.UUID;
import org.cqrs.Event;

public interface EventStore {
    List<Event> getEventsForAggregate(UUID aggregateId);
    void saveEvent(UUID aggregateId, List<Event> uncommittedChanges, long expectedVersion) throws AggregateConcurrencyException;
}
