package org.cqrs;

import java.util.UUID;

public abstract class Message {

    private UUID correlationId;

    public UUID getCorrelationId() {
        return correlationId;
    }

    public void setCorrelationId(UUID correlationId) {
        this.correlationId = correlationId;
    }
}
