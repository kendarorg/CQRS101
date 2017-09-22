package org.cqrs101.shared.Tasks;

import java.util.UUID;
import org.cqrs.Event;

public class TaskTitleVerified implements Event {

    private UUID id;

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }
}
