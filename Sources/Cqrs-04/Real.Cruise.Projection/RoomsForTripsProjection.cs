using Bus;
using Crud;
using Cruise.Events;
using NServiceBus;
using System.Linq;
using System.Threading.Tasks;

namespace Cruise
{
    public class RoomsForTripsProjection:
        IHandleMessages<RoomCreated>,
        IMessageHandler
    {
        private readonly IRepository<RoomsForTripsEntity> _repository;

        public RoomsForTripsProjection( IRepository<RoomsForTripsEntity> repository)
        {
            _repository = repository;
        }

        public Task Handle(RoomCreated evt, IMessageHandlerContext context)
        {
            // ReSharper disable once InvertIf
            if (evt.Success)
            {
                var existingRooms = RetrieveRoomCollection(evt) ?? new RoomsForTripsEntity
                {
                    Class = evt.Class,
                    Count = 0,
                    Id = evt.RoomId,
                    CruiseId = evt.CruiseId
                };

                existingRooms.Count++;
                _repository.Save(existingRooms);
            }
            return Task.CompletedTask;
        }

        private RoomsForTripsEntity RetrieveRoomCollection(RoomCreated evt)
        {
            return _repository.GetAll(room =>
                    room.Class == evt.Class && room.CruiseId == evt.CruiseId)
                .FirstOrDefault();
        }
    }
}