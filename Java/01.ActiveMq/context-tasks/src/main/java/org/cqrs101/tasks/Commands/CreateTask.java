package org.cqrs101.tasks.commands;

import java.util.Date;
import java.util.UUID;
import org.cqrs.Command;

public class CreateTask extends Command {

    private UUID id;
    private String title;
    private String description;
    private int priority;

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public int getPriority() {
        return priority;
    }

    public void setPriority(int priority) {
        this.priority = priority;
    }
}
