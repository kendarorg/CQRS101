package org.cqrs101.orders.commands;

import java.util.UUID;
import org.cqrs.Command;

public class CreateOrder extends Command {

    private UUID id;
    private UUID userId;

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
}
