package org.cqrs101.invoices;

import java.util.Date;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.inject.Inject;
import javax.inject.Named;
import org.cqrs.*;
import org.cqrs101.Repository;
import org.cqrs101.invoices.commands.CompleteInvoice;
import org.cqrs101.invoices.commands.CreateInvoice;
import org.cqrs101.invoices.repositories.Invoice;
import org.cqrs101.shared.invoices.events.InvoiceCompleted;
import org.cqrs101.shared.invoices.events.InvoiceCreated;
import org.cqrs101.shared.customers.CustomerDto;
import org.cqrs101.shared.customers.CustomersService;

@Named("invoicesCommandHandler")
public class InvoicesCommandHandler implements MessageHandler {

    private static final Logger logger = Logger.getLogger(InvoicesCommandHandler.class.getSimpleName());
    private final Repository<Invoice> repository;
    private Bus bus;
    private final CustomersService customersService;

    @Override
    public void register(Bus bus) {
        this.bus = bus;
        this.bus.registerHandler(c -> handle((CreateInvoice) c), CreateInvoice.class, this.getClass());
        this.bus.registerHandler(c -> handle((CompleteInvoice) c), CompleteInvoice.class, this.getClass());
    }

    @Inject
    public InvoicesCommandHandler(Repository<Invoice> invoicesRepository, CustomersService customersService) {
        this.repository = invoicesRepository;
        this.customersService = customersService;
    }

    public void handle(CreateInvoice command) {
        logger.log(Level.INFO, "{0}-CreateInvoice", command.getCorrelationId());
        CustomerDto customer = customersService.getCustomer(command.getCustomerId());

        Date now = new Date();
        Invoice invoiceDao = new Invoice();
        invoiceDao.setId(command.getId());
        invoiceDao.setCustomer(customer);
        invoiceDao.setCreationDate(now);

        repository.save(invoiceDao);
        InvoiceCreated message = new InvoiceCreated();
        message.setId(command.getId());
        message.setCustomerId(customer.getId());
        message.setCreationDate(now);
        bus.send(message);
    }

    public void handle(CompleteInvoice command) {
        logger.log(Level.INFO, "{0}-CompleteInvoice", command.getCorrelationId());
        Date now = new Date();
        Invoice invoiceDao = repository.getById(command.getId());
        invoiceDao.setValue(command.getValue());
        invoiceDao.setCompletionDate(now);

        InvoiceCompleted message = new InvoiceCompleted();
        message.setId(command.getId());
        message.setValue(command.getValue());
        message.setCompletionDate(now);
        bus.send(message);
    }
}
