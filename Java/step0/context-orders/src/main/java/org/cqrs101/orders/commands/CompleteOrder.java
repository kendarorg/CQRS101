package org.cqrs101.orders.commands;

import java.util.Date;
import java.util.UUID;
import org.cqrs.Command;

public class CompleteOrder extends Command {

    private UUID id;
    private float value;
    private Date completionDate;

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

    public Date getCompletionDate() {
        return completionDate;
    }

    public void setCompletionDate(Date completionDate) {
        this.completionDate = completionDate;
    }
}
