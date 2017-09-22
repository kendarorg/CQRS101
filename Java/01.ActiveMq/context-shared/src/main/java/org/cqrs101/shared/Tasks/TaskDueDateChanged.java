package org.cqrs101.shared.Tasks;


import java.util.Date;
import java.util.UUID;
import org.cqrs.Event;

public class TaskDueDateChanged implements Event {

    private UUID id;
    private Date old;
    private Date newItem;
    
    public TaskDueDateChanged()
    {
        
    }
    
    public TaskDueDateChanged(UUID id, Date title) {
        this.id = id;
        this.newItem = title;
    }

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public Date getOld() {
        return old;
    }

    public void setOld(Date old) {
        this.old = old;
    }

    public Date getNew() {
        return newItem;
    }

    public void setNew(Date newItem) {
        this.newItem = newItem;
    }
}
