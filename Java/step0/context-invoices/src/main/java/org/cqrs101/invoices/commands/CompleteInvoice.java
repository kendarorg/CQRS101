package org.cqrs101.invoices.commands;

import java.util.Date;
import java.util.UUID;
import org.cqrs.Command;

public class CompleteInvoice extends Command {

    private UUID id;
    private float value;

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public float getValue() {
        return value;
    }

    public void setValue(float value) {
        this.value = value;
    }
}
