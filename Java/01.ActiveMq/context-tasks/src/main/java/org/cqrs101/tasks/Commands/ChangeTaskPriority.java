package org.cqrs101.tasks.Commands;


import java.util.UUID;
import org.cqrs.Command;


    public class ChangeTaskPriority implements Command
    {
        private UUID id;
        private int priority;

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public int getPriority() {
        return priority;
    }

    public void setPriority(int priority) {
        this.priority = priority;
    }
    }
