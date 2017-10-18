package org.cqrs101.cruds;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import java.util.stream.Collectors;
import javax.inject.Inject;
import javax.inject.Named;
import org.cqrs101.Repository;
import org.cqrs101.RepositoryHelper;

@Named("customersRepository")
public class CustomersRepository implements Repository<Customer> {

    private RepositoryHelper helper;

    @Inject
    public CustomersRepository(RepositoryHelper helper) {
        this.helper = helper.create(Customer.class);
    }

    @Override
    public void delete(UUID id) {
        helper.delete(id);
    }

    @Override
    public List<Customer> getAll() {
        return helper.getAll()
                .stream()
                .map(c -> (Customer) c)
                .collect(Collectors.toList());
    }

    @Override
    public Customer getById(UUID id) {
        return (Customer)helper.getById(id);
    }

    @Override
    public Customer save(Customer item) {
        return (Customer) helper.save(item,
                (obj, id) -> ((Customer) obj).setId(id),
                (obj) -> ((Customer) obj).getId());
    }
}
