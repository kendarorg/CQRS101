package org.cqrs101.shared.customers;

import java.util.UUID;

public class CustomerDto {

    private UUID id;
    private String customerName;

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public String getCustomerName() {
        return customerName;
    }

    public void setCustomerName(String customerName) {
        this.customerName = customerName;
    }
}
