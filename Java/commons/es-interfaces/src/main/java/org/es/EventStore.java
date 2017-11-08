package org.es;

import java.util.List;
import java.util.UUID;

import org.cqrs.Event;

public interface EventStore {
    List<EsEvent> getEventsForAggregate(UUID aggregateId);
    void saveEvent(UUID aggregateId, List<EsEvent> uncommittedChanges, long expectedVersion) throws AggregateConcurrencyException;
    void registerClass(Class messageType);
}
