package org.es;

import java.util.List;
import java.util.UUID;
import org.cqrs.Event;

public class EventDescriptor {
    private UUID id;
    private long version;
    private List<Event> data;

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public long getVersion() {
        return version;
    }

    public void setVersion(long version) {
        this.version = version;
    }

    public List<Event> getData() {
        return data;
    }

    public void setData(List<Event> data) {
        this.data = data;
    }
}
