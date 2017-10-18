package org.cqrs101.orders;

import java.util.Date;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.inject.Named;
import org.cqrs.*;
import org.cqrs101.Repository;
import org.cqrs101.orders.commands.CancelOrder;
import org.cqrs101.orders.commands.CompleteOrder;
import org.cqrs101.orders.commands.CreateOrder;
import org.cqrs101.orders.repositories.Order;
import org.cqrs101.shared.orders.events.OrderCompleted;
import org.cqrs101.shared.orders.events.OrderCreated;
import org.cqrs101.shared.customers.CustomerDto;
import org.cqrs101.shared.customers.CustomersService;

@Named("ordersCommandHandler")
public class OrdersCommandHandler implements MessageHandler {

    private static final Logger logger = Logger.getLogger(OrdersCommandHandler.class.getSimpleName());
    private final Repository<Order> repository;
    private Bus bus;
    private final CustomersService usersService;

    @Override
    public void register(Bus bus) {
        this.bus = bus;
        this.bus.registerHandler(c -> handle((CreateOrder) c), CreateOrder.class, this.getClass());
        this.bus.registerHandler(c -> handle((CompleteOrder) c), CompleteOrder.class, this.getClass());
        this.bus.registerHandler(c -> handle((CancelOrder) c), CancelOrder.class, this.getClass());
    }

    public OrdersCommandHandler(Repository<Order> ordersRepository, CustomersService usersService) {
        this.repository = ordersRepository;
        this.usersService = usersService;
    }

    public void handle(CreateOrder command) {
        logger.log(Level.INFO, "{0}-CreateOrder", command.getCorrelationId());
        CustomerDto user = usersService.getCustomer(command.getCustomerId());

        Date now = new Date();
        Order orderDao = new Order();
        orderDao.setId(command.getId());
        orderDao.setCustomer(user);
        orderDao.setCreationDate(now);

        repository.save(orderDao);
        OrderCreated message = new OrderCreated();
        message.setId(command.getId());
        message.setCustomerId(user.getId());
        message.setCreationDate(now);
        bus.send(message);
    }

    public void handle(CompleteOrder command) {
        logger.log(Level.INFO, "{0}-CompleteOrder", command.getCorrelationId());
        Date now = new Date();
        Order orderDao = repository.getById(command.getId());
        orderDao.setValue(command.getValue());
        orderDao.setCompletionDate(now);

        OrderCompleted message = new OrderCompleted();
        message.setId(command.getId());
        message.setValue(command.getValue());
        message.setCompletionDate(now);
        bus.send(message);
    }

    public void handle(CancelOrder command) {
        logger.log(Level.INFO, "{0}-CancelOrder", command.getCorrelationId());
        Date now = new Date();
        Order orderDao = repository.getById(command.getId());
        orderDao.setCompletionDate(now);
        orderDao.setCanceled(true);

        OrderCompleted message = new OrderCompleted();
        message.setId(command.getId());
        message.setCompletionDate(now);
        bus.send(message);
    }
}
