package org.es;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import java.util.function.Consumer;
import org.cqrs.Event;

public abstract class AggregateRoot {

    private final ConcurrentHashMap<Class, ConcurrentHashMap<Class, Consumer<Object>>> applyFunctions = new ConcurrentHashMap<>();

    protected void registerApply(Consumer<Object> applyFunction, Class messageType) {
        applyFunctions.putIfAbsent(this.getClass(), new ConcurrentHashMap<>());
        applyFunctions.get(this.getClass()).putIfAbsent(messageType, applyFunction);
    }

    private void doApply(Event event) {
        ConcurrentHashMap<Class, Consumer<Object>> aggregateApplier = applyFunctions.get(this.getClass());
        Consumer<Object> functionApplier = aggregateApplier.get(event.getClass());
        functionApplier.accept(event);
    }

    protected AggregateRoot() {
        initializeApply();
    }

    protected abstract void initializeApply();

    private final List<Event> changes = new ArrayList<>();
    private UUID id;

    public UUID getId() {
        return id;
    }

    public List<Event> getUncommittedChanges() {
        return changes;
    }

    public void MarkChangesAsCommitted() {
        changes.clear();
    }

    public void LoadsFromHistory(List<Event> history) {
        for (Event e : history) {
            applyChange(e, false);
        }
    }

    protected void applyChange(Event event) {
        applyChange(event, true);
    }

    private void applyChange(Event event, boolean isNew) {
        doApply(event);
        if (isNew) {
            changes.add(event);
        }
    }
}
