package org.cqrs101.shared.orders.events;

import java.util.Date;
import java.util.UUID;
import org.cqrs.Event;

public class OrderCanceled extends Event {

    private UUID id;
    private Date completionDate;

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
