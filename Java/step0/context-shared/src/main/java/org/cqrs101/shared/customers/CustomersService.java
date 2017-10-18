package org.cqrs101.shared.customers;

import java.util.UUID;

public interface CustomersService {

    CustomerDto getCustomer(UUID id);
}
