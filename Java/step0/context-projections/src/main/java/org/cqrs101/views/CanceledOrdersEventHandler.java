package org.cqrs101.views;

import org.cqrs.Bus;
import org.cqrs.MessageHandler;
import org.cqrs101.Repository;
import org.cqrs101.shared.customers.CustomerDto;
import org.cqrs101.shared.customers.CustomersService;
import org.cqrs101.shared.orders.events.OrderCanceled;
import org.cqrs101.views.repositories.CanceledOrder;
import org.cqrs101.views.repositories.InProgressOrder;

import javax.inject.Inject;
import javax.inject.Named;
import java.util.logging.Level;
import java.util.logging.Logger;

@Named("canceledOrdersEventHandler")
public class CanceledOrdersEventHandler implements MessageHandler {

    private static final Logger logger = Logger.getLogger(CanceledOrdersEventHandler.class.getSimpleName());
    private Bus bus;
    private final CustomersService usersService;
    private final Repository<CanceledOrder> repository;

    @Override
    public void register(Bus bus) {
        this.bus = bus;
        this.bus.registerHandler(m -> handle((OrderCanceled) m), OrderCanceled.class, this.getClass());
    }

    @Inject
    public CanceledOrdersEventHandler(CustomersService usersService, Repository<CanceledOrder> repository) {
        this.usersService = usersService;
        this.repository = repository;
    }

    public void handle(OrderCanceled message) {
        logger.log(Level.INFO, "{0}-OrderCanceled", message.getCorrelationId());
        CustomerDto user = usersService.getCustomer(message.getCustomerId());
        CanceledOrder order = new CanceledOrder();
        order.setId(message.getId());
        order.setCreationDate(message.getCreationDate());
        order.setCancellationDate(message.getCancellationDate());
        order.setCustomerName(user.getCustomerName());
        order.setCustomerId(user.getId());

        repository.save(order);
    }
}
