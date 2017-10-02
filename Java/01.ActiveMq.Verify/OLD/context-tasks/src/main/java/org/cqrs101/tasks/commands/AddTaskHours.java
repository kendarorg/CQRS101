package org.cqrs101.tasks.commands;

import java.util.Date;
import java.util.UUID;
import org.cqrs.Command;

public class AddTaskHours extends Command {

    private UUID id;
    private int taskHours;

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public int getTaskHours() {
        return taskHours;
    }

    public void setTaskHours(int TaskHours) {
        this.taskHours = TaskHours;
    }
}
