package org.cqrs101.views;

import org.cqrs101.shared.customers.CustomerDto;
import org.cqrs101.shared.customers.CustomersService;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.inject.Inject;
import javax.inject.Named;
import org.cqrs.Bus;
import org.cqrs.MessageHandler;
import org.cqrs101.Repository;
import org.cqrs101.shared.orders.events.OrderCompleted;
import org.cqrs101.shared.orders.events.OrderCreated;
import org.cqrs101.views.repositories.*;

@Named("completedOrdersEventHandler")
public class CompletedOrdersEventHandler implements MessageHandler {

    private static final Logger logger = Logger.getLogger(CompletedOrdersEventHandler.class.getSimpleName());
    private Bus bus;
    private final CustomersService usersService;
    private final Repository<CompletedOrder> repository;

    @Override
    public void register(Bus bus) {
        this.bus = bus;
        this.bus.registerHandler(m -> handle((OrderCompleted) m), OrderCompleted.class, this.getClass());
    }

    @Inject
    public CompletedOrdersEventHandler(CustomersService usersService, Repository<CompletedOrder> repository) {
        this.usersService = usersService;
        this.repository = repository;
    }

    public void handle(OrderCompleted message) {
        logger.log(Level.INFO, "{0}-OrderCompleted", message.getCorrelationId());
        CustomerDto user = usersService.getCustomer(message.getCustomerId());
        CompletedOrder order = new CompletedOrder();
        order.setId(message.getId());
        order.setCreationDate(message.getCreationDate());
        order.setCoompletionDate(message.getCompletionDate());
        order.setCustomerName(user.getCustomerName());
        order.setCustomerId(user.getId());
        
        repository.save(order);
    }
}
