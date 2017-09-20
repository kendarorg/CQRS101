package org.tasks.Commands;

import java.util.UUID;
import org.cqrs.Command;

public class ChangeTaskType implements Command {

    private UUID id;
    private String typeCode;

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public String getTypeCode() {
        return typeCode;
    }

    public void setTypeCode(String typeCode) {
        this.typeCode = typeCode;
    }
}
