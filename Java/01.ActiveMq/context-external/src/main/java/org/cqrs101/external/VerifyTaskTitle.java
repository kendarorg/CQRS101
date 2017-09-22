package org.cqrs101.external;

import java.util.UUID;
import org.cqrs.Command;

public class VerifyTaskTitle implements Command {

    private UUID id;
    private String title;

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }
}
