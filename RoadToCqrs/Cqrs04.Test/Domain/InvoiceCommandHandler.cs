using Cqrs01.Test.Infrastructure;
using Cqrs02.Test.Infrastructure;
using Cqrs04.Test.Domains.Invoices.Commands;
using Cqrs04.Test.Domains.Invoices.Customers;
using Cqrs04.Test.Domains.Invoices.Events;

namespace Cqrs04.Test.Domains.Invoices
{
    public class InvoiceCommandHandler
    {
        private readonly ICustomersServiceProxy _customersServices;

        private readonly EntityStorage _entityStorage;
        private readonly Bus _bus;

        public InvoiceCommandHandler(Bus bus, EntityStorage entityStorage, ICustomersServiceProxy customerServices)
        {
            _entityStorage = entityStorage;
            _bus = bus;
            _bus.RegisterQueue<CreateInvoice>(Handle);
            _bus.RegisterQueue<EmitInvoice>(Handle);
            _customersServices = customerServices;
        }

        public void Handle(CreateInvoice command)
        {
            var customer = _customersServices.GetCustomerById(command.CustomerId);
            if (customer != null)
            {
                var aggregate = new InvoiceAggregateRoot(command.InvoiceId, command.CustomerId);
                _entityStorage.Save(command.InvoiceId, aggregate);
            }
        }

        public void Handle(EmitInvoice command)
        {
            var entity = _entityStorage.GetById<InvoiceEntity>(command.InvoiceId);
            var customer = _customersServices.GetCustomerById(entity.CustomerId);
            var aggregate = new InvoiceAggregateRoot(entity);
            var address = _customersServices.GetBillingAddressForId(customer.BillingAddressId);
            aggregate.EmitInvoice(address);
            _entityStorage.Save(command.InvoiceId, aggregate, command.ExpectedVersion);
        }
    }
}