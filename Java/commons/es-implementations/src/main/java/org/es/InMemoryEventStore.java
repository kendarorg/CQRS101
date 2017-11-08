package org.es;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import javax.inject.Named;
import org.cqrs.Event;

@Named("eventStore")
public class InMemoryEventStore implements EventStore {

    private static ConcurrentHashMap<UUID, List<EsEvent>> storage = new ConcurrentHashMap<>();

    @Override
    public List<EsEvent> getEventsForAggregate(UUID aggregateId) {
        List<EsEvent> result = new ArrayList<>();
        if (storage.containsKey(aggregateId)) {
            for (EsEvent eventDescriptor : storage.get(aggregateId)) {
                result.add(eventDescriptor);
            }
        }
        return result;
    }

    @Override
    public void saveEvent(UUID aggregateId, List<EsEvent> uncommittedChanges, long expectedVersion) throws AggregateConcurrencyException {
        storage.putIfAbsent(aggregateId, new ArrayList<>());
        List<EsEvent> data = storage.get(aggregateId);

        if (!data.isEmpty()) {
            EsEvent last = data.get(data.size() - 1);
            if (last.getVersion() != expectedVersion) {
                throw new AggregateConcurrencyException();
            }
        }

        long version = expectedVersion+1;

        for(EsEvent uncommittedChange : uncommittedChanges){
            uncommittedChange.setVersion(version);
            data.add(uncommittedChange);
        }
    }

    @Override
    public void registerClass(Class messageType) {

    }

}
