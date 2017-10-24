package org.cqrs101.shared.invoices.events;

import java.util.Date;
import java.util.UUID;
import org.cqrs.Event;

public class InvoiceCreated extends Event {

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

    public void setCustomerId(UUID customerId) {
        this.customerId = customerId;
    }

    public Date getCreationDate() {
        return creationDate;
    }

    public void setCreationDate(Date creationDate) {
        this.creationDate = creationDate;
    }
}
