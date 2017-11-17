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
import org.cqrs101.shared.invoices.events.InvoiceCompleted;
import org.cqrs101.shared.invoices.events.InvoiceCreated;
import org.cqrs101.views.repositories.*;

@Named("inProgressInvoicesEventHandler")
public class InProgressInvoicesEventHandler implements MessageHandler {

    private static final Logger logger = Logger.getLogger(InProgressInvoicesEventHandler.class.getSimpleName());
    private Bus bus;
    private final CustomersService customersService;
    private final Repository<InProgressInvoice> repository;

    @Override
    public void register(Bus bus) {
        this.bus = bus;
        this.bus.registerHandler(m -> handle((InvoiceCreated) m), InvoiceCreated.class, this.getClass());
        this.bus.registerHandler(m -> handle((InvoiceCompleted) m), InvoiceCompleted.class, this.getClass());
   }

    @Inject
    public InProgressInvoicesEventHandler(CustomersService customersService, Repository<InProgressInvoice> repository) {
        this.customersService = customersService;
        this.repository = repository;
    }

    public void handle(InvoiceCreated message) {
        logger.log(Level.INFO, "{0}-InvoiceCreated", message.getCorrelationId());
        CustomerDto customer = customersService.getCustomer(message.getCustomerId());
        InProgressInvoice invoice = new InProgressInvoice();
        invoice.setId(message.getId());
        invoice.setCreationDate(message.getCreationDate());
        invoice.setCustomerName(customer.getName());
        invoice.setCustomerId(customer.getId());

        repository.save(invoice);
    }

    public void handle(InvoiceCompleted message) {
        logger.log(Level.INFO, "{0}-InvoiceCompleted", message.getCorrelationId());
        repository.delete(message.getId());
    }
}
