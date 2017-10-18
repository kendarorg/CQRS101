package org.cqrs101.shared.orders.events;

import java.util.Date;
import java.util.UUID;
import org.cqrs.Event;

public class OrderCreated extends Event {

    private UUID id;
    private UUID customerId;
    private Date creationDate;

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public UUID getCustomerId() {
        return customerId;
    }

    public void setCustomerId(UUID userId) {
        this.customerId = userId;
    }

    public Date getCreationDate() {
        return creationDate;
    }

    public void setCreationDate(Date creationDate) {
        this.creationDate = creationDate;
    }
}
