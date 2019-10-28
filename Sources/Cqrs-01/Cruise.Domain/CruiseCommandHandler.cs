using System;
using System.Collections.Generic;
using System.Linq;
using Bus;
using Crud;
using Cruise.Commands;
using Cruise.Events;

// ReSharper disable MemberCanBePrivate.Global

namespace Cruise
{
    public class CruiseCommandHandler
    {
        private readonly IBus _bus;
        private readonly IOptimisticRepository<CruiseEntity> _repository;

        public CruiseCommandHandler(IBus bus, IOptimisticRepository<CruiseEntity> repository)
        {
            _bus = bus;
            _bus.RegisterQueue<CreateCruise>(Handle);
            _bus.RegisterQueue<CreateRoom>(Handle);
            _repository = repository;
        }

        public void Handle(CreateCruise createCruiseCommand)
        {
            var newCruise = new CruiseEntity(Guid.NewGuid(), 0, createCruiseCommand.Name, new List<Room>());
            _repository.Save(newCruise);
            var cruiseCreatedEvent = new CruiseCreated(newCruise.Name, newCruise.Id);
            _bus.Send(cruiseCreatedEvent);
        }

        public void Handle(CreateRoom addCruiseRoom)
        {
            var cruise = _repository.GetById(addCruiseRoom.CruiseId);
            var result = new RoomCreated(
                cruise.Id,
                addCruiseRoom.Number, addCruiseRoom.Class, addCruiseRoom.Beds,
                false, Guid.NewGuid());

            if (NoRoomsWithSameNumber(addCruiseRoom, cruise))
            {
                var newRoom = new Room(
                    cruise.Id,
                    addCruiseRoom.Number, addCruiseRoom.Class, addCruiseRoom.Beds,
                    result.RoomId);
                cruise.Rooms.Add(newRoom);
                _repository.Save(cruise);
                result.Success = true;
            }

            _bus.Send(result);
        }

        private static bool NoRoomsWithSameNumber(CreateRoom addCruiseRoom, CruiseEntity cruise)
        {
            return cruise.Rooms.All(room => room.Number != addCruiseRoom.Number);
        }
    }
}