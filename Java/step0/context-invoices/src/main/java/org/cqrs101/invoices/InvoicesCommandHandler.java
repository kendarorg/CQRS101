package org.cqrs101.invoices;

import java.util.Date;
import java.util.logging.Level;
import java.util.logging.Logger;
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
    private final CustomersService usersService;

    @Override
    public void register(Bus bus) {
        this.bus = bus;
        this.bus.registerHandler(c -> handle((CreateInvoice) c), CreateInvoice.class, this.getClass());
        this.bus.registerHandler(c -> handle((CompleteInvoice) c), CompleteInvoice.class, this.getClass());
    }

    public InvoicesCommandHandler(Repository<Invoice> invoicesRepository, CustomersService usersService) {
        this.repository = invoicesRepository;
        this.usersService = usersService;
    }

    public void handle(CreateInvoice command) {
        logger.log(Level.INFO, "{0}-CreateInvoice", command.getCorrelationId());
        CustomerDto user = usersService.getCustomer(command.getCustomerId());

        Date now = new Date();
        Invoice invoiceDao = new Invoice();
        invoiceDao.setId(command.getId());
        invoiceDao.setCustomer(user);
        invoiceDao.setCreationDate(now);

        repository.save(invoiceDao);
        InvoiceCreated message = new InvoiceCreated();
        message.setId(command.getId());
        message.setCustomerId(user.getId());
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
