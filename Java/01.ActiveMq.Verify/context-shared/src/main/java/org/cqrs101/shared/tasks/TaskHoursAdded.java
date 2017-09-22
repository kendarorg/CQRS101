package org.cqrs101.shared.tasks;


import java.util.Date;
import java.util.UUID;
import org.cqrs.Event;

public class TaskHoursAdded extends Event { 

    private UUID id;
    private int old;
    private int newItem;
    
    public TaskHoursAdded()
    {
        
    }
    
    public TaskHoursAdded(UUID id, int title) {
        this.id = id;
        this.newItem = title;
    }

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public int getOld() {
        return old;
    }

    public void setOld(int old) {
        this.old = old;
    }

    public int getNew() {
        return newItem;
    }

    public void setNew(int newItem) {
        this.newItem = newItem;
    }
}
