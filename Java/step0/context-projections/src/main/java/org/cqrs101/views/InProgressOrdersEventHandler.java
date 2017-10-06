package org.cqrs101.views;

import java.util.logging.Level;
import java.util.logging.Logger;
import javax.inject.Inject;
import javax.inject.Named;
import org.cqrs.Bus;
import org.cqrs.MessageHandler;
import org.cqrs101.Repository;
import org.cqrs101.shared.orders.events.OrderCreated;
import org.cqrs101.views.repositories.*;
import org.cqrs101.shared.users.*;

@Named("doneTasksEventHandler")
public class InProgressOrdersEventHandler implements MessageHandler {

    private static final Logger logger = Logger.getLogger(InProgressOrdersEventHandler.class.getSimpleName());
    private Bus bus;
    private final UsersService usersService;
    private final Repository<InProgressOrder> repository;

    @Override
    public void register(Bus bus) {
        this.bus = bus;
        this.bus.registerHandler(m -> handle((OrderCreated) m), OrderCreated.class, this.getClass());
    }

    @Inject
    public InProgressOrdersEventHandler(UsersService usersService, Repository<InProgressOrder> repository) {
        this.usersService = usersService;
        this.repository = repository;
    }

    public void handle(OrderCreated message) {
        logger.log(Level.INFO, "{0}-TaskCompleted", message.getCorrelationId());
        UserDto user = usersService.getUser(message.getUserId());
        InProgressOrder order = new InProgressOrder();
        order.setId(message.getId());
        order.setCreationDate(message.getCreationDate());
        order.setUserName(user.getUsername());

        repository.save(order);
    }
}
