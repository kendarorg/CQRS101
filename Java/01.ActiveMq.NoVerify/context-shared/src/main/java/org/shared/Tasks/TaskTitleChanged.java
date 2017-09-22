package org.shared.Tasks;


import java.util.UUID;
import org.cqrs.Event;

public class TaskTitleChanged implements Event {

    private UUID id;
    private String old;
    private String newItem;

    public TaskTitleChanged()
    {
        
    }
    
    public TaskTitleChanged(UUID id, String title) {
        this.id = id;
        this.newItem = title;
    }

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public String getOld() {
        return old;
    }

    public void setOld(String old) {
        this.old = old;
    }

    public String getNew() {
        return newItem;
    }

    public void setNew(String newItem) {
        this.newItem = newItem;
    }
}
