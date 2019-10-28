using Bus;
using Crud;
using Cruise.Events;
using System.Linq;

namespace Cruise
{
    public class RoomsForTripsProjection
    {
        private readonly IBus _bus;
        private readonly IRepository<RoomsForTripsEntity> _repository;

        public RoomsForTripsProjection(IBus bus, IRepository<RoomsForTripsEntity> repository)
        {
            _bus = bus;
            _bus.RegisterTopic<RoomCreated>(Handle);
            _repository = repository;
        }

        public void Handle(RoomCreated evt)
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
        }

        private RoomsForTripsEntity RetrieveRoomCollection(RoomCreated evt)
        {
            return _repository.GetAll(room =>
                    room.Class == evt.Class && room.CruiseId == evt.CruiseId)
                .FirstOrDefault();
        }
    }
}