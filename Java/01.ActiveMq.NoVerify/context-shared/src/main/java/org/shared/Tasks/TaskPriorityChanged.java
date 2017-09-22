package org.shared.Tasks;


import java.util.UUID;
import org.cqrs.Event;

public class TaskPriorityChanged implements Event {

    private UUID id;
    private int old;
    private int newItem;
    
    public TaskPriorityChanged()
    {
        
    }
    
    public TaskPriorityChanged(UUID id, int title) {
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
