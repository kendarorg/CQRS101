package org.cqrs101.invoices.commands;

import java.util.UUID;
import org.cqrs.Command;

public class CreateInvoice extends Command {

    private UUID id;
    private UUID customerId;

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
}
