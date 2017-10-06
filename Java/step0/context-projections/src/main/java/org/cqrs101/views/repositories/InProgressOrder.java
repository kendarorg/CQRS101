package org.cqrs101.views.repositories;

import java.util.Date;
import java.util.UUID;

public class InProgressOrder {

    private UUID id;
    private Date creationDate;
    private String userName;

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public Date getCreationDate() {
        return creationDate;
    }

    public void setCreationDate(Date creationDate) {
        this.creationDate = creationDate;
    }

    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }
}
