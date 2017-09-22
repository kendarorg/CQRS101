package org.cqrs101.tasks.commands;

import java.util.UUID;
import org.cqrs.Command;

public class ChangeTaskDescription extends Command {

    private UUID id;
    private String description;

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }
}
