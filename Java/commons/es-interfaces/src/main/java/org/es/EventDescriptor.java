package org.es;

import java.util.List;
import java.util.UUID;
import org.cqrs.Event;

public class EventDescriptor {
    private UUID id;
    private long version;
    private long commitId;
    private Event data;

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

    public Event getData() {
        return data;
    }

    public void setData(Event data) {
        this.data = data;
    }

    public long getCommitId() {
        return commitId;
    }

    public void setCommitId(long commitId) {
        this.commitId = commitId;
    }
}
