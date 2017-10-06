package org.cqrs101.shared.orders.events;

import java.util.Date;
import java.util.UUID;
import org.cqrs.Event;

public class OrderCreated extends Event {

    private UUID id;
    private UUID userId;
    private Date creationDate;

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public UUID getUserId() {
        return userId;
    }

    public void setUserId(UUID userId) {
        this.userId = userId;
    }

    public Date getCreationDate() {
        return creationDate;
    }

    public void setCreationDate(Date creationDate) {
        this.creationDate = creationDate;
    }
}
