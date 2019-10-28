using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bus;
using Crud;
using Cruise.Commands;
using Cruise.Events;
using NServiceBus;

namespace Cruise
{
    public class CruiseCommandHandler:
        IHandleMessages<CreateCruise>,
        IHandleMessages<CreateRoom>,
        IMessageHandler
    {
        private readonly IOptimisticRepository<CruiseEntity> _repository;

        public CruiseCommandHandler(IOptimisticRepository<CruiseEntity> repository)
        {
            _repository = repository;
        }

        public Task Handle(CreateCruise createCruiseCommand, IMessageHandlerContext context)
        {
            var newCruise = new CruiseEntity(Guid.NewGuid(), 0, createCruiseCommand.Name, new List<Room>());
            _repository.Save(newCruise);
            var cruiseCreatedEvent = new CruiseCreated(newCruise.Name, newCruise.Id);
            return context.Publish(cruiseCreatedEvent);
        }

        public Task Handle(CreateRoom addCruiseRoom, IMessageHandlerContext context)
        {
            var cruise = _repository.GetById(addCruiseRoom.CruiseId);
            var roomCreated = new RoomCreated(
                cruise.Id,
                addCruiseRoom.Number, addCruiseRoom.Class, addCruiseRoom.Beds,
                false, Guid.NewGuid());

            if (NoRoomsWithSameNumber(addCruiseRoom, cruise))
            {
                var newRoom = new Room(
                    cruise.Id,
                    addCruiseRoom.Number, addCruiseRoom.Class, addCruiseRoom.Beds,
                    roomCreated.RoomId);
                cruise.Rooms.Add(newRoom);
                _repository.Save(cruise);
                roomCreated.Success = true;
            }

            return context.Publish(roomCreated);
        }

        private static bool NoRoomsWithSameNumber(CreateRoom addCruiseRoom, CruiseEntity cruise)
        {
            return cruise.Rooms.All(room => room.Number != addCruiseRoom.Number);
        }
    }
}