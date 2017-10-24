package org.cqrs101.views.repositories;

import java.util.Date;
import java.util.UUID;

public class CompletedInvoice {

    private UUID id;
    private Date creationDate;
    private Date coompletionDate;
    private double value;
    private UUID customerId;
    private String customerName;

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

    public Date getCoompletionDate() {
        return coompletionDate;
    }

    public void setCoompletionDate(Date coompletionDate) {
        this.coompletionDate = coompletionDate;
    }

    public double getValue() {
        return value;
    }

    public void setValue(double value) {
        this.value = value;
    }

    public UUID getCustomerId() {
        return customerId;
    }

    public void setCustomerId(UUID customerId) {
        this.customerId = customerId;
    }

    public String getCustomerName() {
        return customerName;
    }

    public void setCustomerName(String customerName) {
        this.customerName = customerName;
    }


}
