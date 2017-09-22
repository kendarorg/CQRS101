package org.cqrs101.shared.tasks;

import java.util.Date;
import java.util.UUID;
import org.cqrs.Event;

public class TaskCreated extends Event {
    private UUID id;
    private Date creationDate;
    private TaskPriorityChanged prioritySet;
    private TaskDescriptionChanged descriptionSet;
    private TaskTitleChanged titleSet;

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public Date getCreationDate() {
        return creationDate;
    }

    public void setCreationDate(Date creationDate) {
        this.creationDate = creationDate;
    }

    public TaskPriorityChanged getPrioritySet() {
        return prioritySet;
    }

    public void setPrioritySet(TaskPriorityChanged prioritySet) {
        this.prioritySet = prioritySet;
    }

    public TaskDescriptionChanged getDescriptionSet() {
        return descriptionSet;
    }

    public void setDescriptionSet(TaskDescriptionChanged descriptionSet) {
        this.descriptionSet = descriptionSet;
    }

    public TaskTitleChanged getTitleSet() {
        return titleSet;
    }

    public void setTitleSet(TaskTitleChanged titleSet) {
        this.titleSet = titleSet;
    }
}
