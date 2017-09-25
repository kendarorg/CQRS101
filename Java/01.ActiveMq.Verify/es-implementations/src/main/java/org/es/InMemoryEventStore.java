package org.es;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import javax.inject.Named;
import org.cqrs.Event;

@Named("eventStore")
public class InMemoryEventStore implements EventStore {

    private static ConcurrentHashMap<UUID, List<EventDescriptor>> storage = new ConcurrentHashMap<>();

    @Override
    public List<Event> getEventsForAggregate(UUID aggregateId) {
        List<Event> result = new ArrayList<>();
        if (storage.containsKey(aggregateId)) {
            for (EventDescriptor eventDescriptor : storage.get(aggregateId)) {
                result.addAll(eventDescriptor.getData());
            }
        }
        return result;
    }

    @Override
    public void saveEvent(UUID aggregateId, List<Event> uncommittedChanges, long expectedVersion) throws AggregateConcurrencyException {
        storage.putIfAbsent(aggregateId, new ArrayList<>());
        List<EventDescriptor> data = storage.get(aggregateId);

        if (!data.isEmpty()) {
            EventDescriptor last = data.get(data.size() - 1);
            if (last.getVersion() != expectedVersion) {
                throw new AggregateConcurrencyException();
            }
        }

        EventDescriptor target = new EventDescriptor();
        target.setVersion(expectedVersion + 1);
        target.setData(uncommittedChanges);
        data.add(target);
    }

}
