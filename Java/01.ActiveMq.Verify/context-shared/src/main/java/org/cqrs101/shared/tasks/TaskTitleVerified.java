package org.cqrs101.shared.tasks;

import java.util.UUID;
import org.cqrs.Event;

public class TaskTitleVerified extends Event {

    private UUID id;

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }
}
