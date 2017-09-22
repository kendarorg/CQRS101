package org.tasks.Commands;

import java.util.Date;
import java.util.UUID;
import org.cqrs.Command;


    public class ChangeTaskDueDate implements Command
    {
        private UUID id;
        private Date DueDate;

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public Date getDueDate() {
        return DueDate;
    }

    public void setDueDate(Date DueDate) {
        this.DueDate = DueDate;
    }
    }