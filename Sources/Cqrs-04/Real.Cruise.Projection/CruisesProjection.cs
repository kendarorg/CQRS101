using Bus;
using Crud;
using Cruise.Events;
using NServiceBus;
using System.Threading.Tasks;

namespace Cruise
{
    public class CruisesProjection :
        IHandleMessages<CruiseCreated>,
        IMessageHandler
    {
        private readonly IRepository<CruiseProjectionEntity> _repository;

        public CruisesProjection(IRepository<CruiseProjectionEntity> repository)
        {
            _repository = repository;
        }
        public Task Handle(CruiseCreated message, IMessageHandlerContext context)
        {
            _repository.Save(new CruiseProjectionEntity
            {
                Id = message.CruiseId,
                Name = message.Name
            });
            return Task.CompletedTask;
        }
    }
}
