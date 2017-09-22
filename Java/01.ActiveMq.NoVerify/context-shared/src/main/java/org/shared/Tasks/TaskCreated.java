package org.shared.Tasks;


import java.util.Date;
import java.util.UUID;
import org.cqrs.Event;

public class TaskCreated implements Event {

    private UUID id;
    private Date creationDate;
    private TaskDueDateChanged dueDateSet;
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

    public TaskDueDateChanged getDueDateSet() {
        return dueDateSet;
    }

    public void setDueDateSet(TaskDueDateChanged dueDateSet) {
        this.dueDateSet = dueDateSet;
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
