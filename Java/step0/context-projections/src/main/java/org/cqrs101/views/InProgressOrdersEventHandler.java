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
import org.cqrs101.shared.orders.events.OrderCanceled;
import org.cqrs101.shared.orders.events.OrderCompleted;
import org.cqrs101.shared.orders.events.OrderCreated;
import org.cqrs101.views.repositories.*;

@Named("inProgressOrdersEventHandler")
public class InProgressOrdersEventHandler implements MessageHandler {

    private static final Logger logger = Logger.getLogger(InProgressOrdersEventHandler.class.getSimpleName());
    private Bus bus;
    private final CustomersService usersService;
    private final Repository<InProgressOrder> repository;

    @Override
    public void register(Bus bus) {
        this.bus = bus;
        this.bus.registerHandler(m -> handle((OrderCreated) m), OrderCreated.class, this.getClass());
        this.bus.registerHandler(m -> handle((OrderCompleted) m), OrderCompleted.class, this.getClass());
        this.bus.registerHandler(m -> handle((OrderCanceled) m), OrderCanceled.class, this.getClass());
    }

    @Inject
    public InProgressOrdersEventHandler(CustomersService usersService, Repository<InProgressOrder> repository) {
        this.usersService = usersService;
        this.repository = repository;
    }

    public void handle(OrderCreated message) {
        logger.log(Level.INFO, "{0}-OrderCompleted", message.getCorrelationId());
        CustomerDto user = usersService.getCustomer(message.getCustomerId());
        InProgressOrder order = new InProgressOrder();
        order.setId(message.getId());
        order.setCreationDate(message.getCreationDate());
        order.setCustomerName(user.getCustomerName());
        order.setCustomerId(user.getId());

        repository.save(order);
    }

    public void handle(OrderCompleted message) {
        logger.log(Level.INFO, "{0}-OrderCompleted", message.getCorrelationId());
        repository.delete(message.getId());
    }

    public void handle(OrderCanceled message) {
        logger.log(Level.INFO, "{0}-OrderCanceled", message.getCorrelationId());
        repository.delete(message.getId());
    }
}
