package org.cqrs101.tasks.commands;

import java.util.UUID;
import org.cqrs.Command;

public class CompleteTask extends Command {

    private UUID id;

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }
}
