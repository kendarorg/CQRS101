package org.commons.Tasks;

import java.util.Date;
import java.util.UUID;
import org.cqrs.Event;

public class TaskCompleted implements Event {

    private UUID id;
    private Date completionDate;
    
    public TaskCompleted(){
        
    }
    
    public TaskCompleted(UUID id, Date completionDate){
        this.id = id;
        this.completionDate = completionDate;
    }

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public Date getCompletionDate() {
        return completionDate;
    }

    public void setCompletionDate(Date completionDate) {
        this.completionDate = completionDate;
    }

}
