package org.cqrs101.cruds;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import java.util.stream.Collectors;
import javax.inject.Inject;
import javax.inject.Named;

import org.cqrs101.BaseRepository;
import org.cqrs101.Repository;
import org.cqrs101.RepositoryHelper;

@Named("customersRepository")
public class CustomersRepository extends BaseRepository<Customer> {

    @Inject
    public CustomersRepository(RepositoryHelper helper) {
        super(helper);
    }

    @Override
    public Customer save(Customer item) {
        return (Customer) helper.save(item,
                (obj, id) -> ((Customer) obj).setId(id),
                (obj) -> ((Customer) obj).getId());
    }
}
