package org.cqrs101.orders.commands;

import java.util.Date;
import java.util.UUID;
import org.cqrs.Command;

public class CancelOrder extends Command {

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
