package org.cqrs101.orders.repositories;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;
import java.util.stream.Collectors;
import javax.inject.Named;
import org.cqrs101.Repository;
import org.cqrs101.RepositoryHelper;

@Named("ordersRepository")
public class OrdersRepository implements Repository<Order> {

    private RepositoryHelper helper;

    public OrdersRepository(RepositoryHelper helper) {
        this.helper = helper.create(Order.class);
    }

    @Override
    public void delete(UUID id) {
        helper.delete(id);
    }

    @Override
    public List<Order> getAll() {
        return helper.getAll()
                .stream()
                .map(c -> (Order) c)
                .collect(Collectors.toList());
    }

    @Override
    public Order getById(UUID id) {
        return (Order)helper.getById(id);
    }

    @Override
    public Order save(Order item) {
        return (Order) helper.save(item, 
                (obj, id) -> ((Order) obj).setId(id), 
                (obj) -> ((Order) obj).getId());
    }
}
