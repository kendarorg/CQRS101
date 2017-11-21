package unit;

import org.cqrs.Bus;
import org.cqrs.Message;
import org.cqrs101.Repository;
import org.cqrs101.invoices.InvoicesCommandHandler;
import org.cqrs101.invoices.commands.CompleteInvoice;
import org.cqrs101.invoices.commands.CreateInvoice;
import org.cqrs101.invoices.repositories.Invoice;
import org.cqrs101.shared.customers.CustomerDto;
import org.cqrs101.shared.customers.CustomersService;
import org.cqrs101.shared.invoices.events.InvoiceCompleted;
import org.cqrs101.shared.invoices.events.InvoiceCreated;
import org.junit.Before;
import org.junit.Test;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;


import static org.junit.Assert.*;
import static org.mockito.Matchers.any;
import static org.mockito.Mockito.*;

public class CommandHandlerTest {
    private Repository<Invoice> invoicesRepository;
    private CustomersService customersService;
    private InvoicesCommandHandler target;
    private Bus bus;
    private List<Message> messages;

    @Before
    public void setUp() {
        invoicesRepository = (Repository<Invoice>) mock(Repository.class);
        customersService = mock(CustomersService.class);
        bus = mock(Bus.class);
        messages = new ArrayList<>();
        doAnswer(invocation -> {
            Message arg0 = (Message)invocation.getArguments()[0];
            messages.add(arg0);
            return null;
        }).when(bus).send(any(Message.class));

        target = new InvoicesCommandHandler(invoicesRepository, customersService);
        target.register(bus);
    }

    @Test
    public void shouldGenerateInvoiceCreatedWhenCreating() {
        CustomerDto customer = new CustomerDto();
        customer.setId(UUID.randomUUID());
        CreateInvoice command = new CreateInvoice();
        when(customersService.getCustomer(any(UUID.class)))
                .thenReturn(customer);

        target.handle(command);

        assertEquals(1,messages.size());
        assertSame(InvoiceCreated.class,messages.get(0).getClass());
    }

    @Test
    public void shouldGenerateInvoiceCompletedWhenCompleting() {
        CompleteInvoice command = new CompleteInvoice();
        Invoice invoice = new Invoice();
        invoice.setCustomer(new CustomerDto());
        when(invoicesRepository.getById(any(UUID.class)))
                .thenReturn(invoice);

        target.handle(command);

        assertEquals(1,messages.size());
        assertSame(InvoiceCompleted.class,messages.get(0).getClass());
    }
}