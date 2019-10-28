using Bus;
using Crud;
using Customer.Services;
using Invoice.Commands;
using Invoice.Events;
using Invoice.Externals;

namespace Invoice
{
    public class InvoiceCommandHandler
    {
        private readonly IBus _bus;
        private readonly IOptimisticRepository<InvoiceEntity> _repository;
        private readonly ICustomerServices _customersServices;

        public InvoiceCommandHandler(IBus bus, IOptimisticRepository<InvoiceEntity> repository, 
            ICustomerServices customersServices)
        {
            _bus = bus;
            _bus.RegisterQueue<CreateInvoice>(Handle);
            _bus.RegisterQueue<EmitInvoice>(Handle);
            _repository = repository;
            _customersServices = customersServices;
        }

        public void Handle(CreateInvoice command)
        {
            var customer = _customersServices.GetCustomerById(command.CustomerId);
            var evt = new InvoiceCreated(command.InvoiceId, command.CustomerId, false);
            
            if (customer != null)
            {
                var newInvoice = new InvoiceEntity
                {
                    Id = command.InvoiceId,
                    CustomerId = command.CustomerId
                };
                _repository.Save(newInvoice);
                evt.Success = true;
            }
            
            _bus.Send(evt);
        }

        public void Handle(EmitInvoice command)
        {
            var invoice = _repository.GetById(command.InvoiceId);
            var address = _customersServices.GetBillingAddressForId(invoice.CustomerId);
            CopyAddressToInvoice(invoice, address);
            _repository.Save(invoice);
            _bus.Send(new InvoiceEmitted(invoice.Id, true,
                invoice.CustomerId, invoice.City, invoice.Country,
                invoice.Name, invoice.Street, invoice.Zip));
        }

        private static void CopyAddressToInvoice(InvoiceEntity invoice, BillingAddressData address)
        {
            invoice.City = address.City;
            invoice.Country = address.Country;
            invoice.Name = address.Name;
            invoice.Street = address.Street;
            invoice.Zip = address.Zip;
        }
    }
}