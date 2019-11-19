using Cqrs01.Test.Domain.Events;
using Cqrs01.Test.Infrastructure;
using System;
using System.Linq;

namespace Cqrs01.Test.Domain
{
    public class CruiseAggregateRoot : AggregateRoot<CruiseEntity>
    {

        public CruiseAggregateRoot(CruiseEntity entity):base(entity)
        {
            
        }
        public CruiseAggregateRoot(Guid id, string name):base()
        {
            Entity = new CruiseEntity(id, name);
            Publish(new CruiseCreated
            {
                CruiseId = Entity.Id,
                Name = Entity.Name
            });
        }

        public void AddRoom(int number, int category)
        {
            CheckForDuplicateRoomNumbers(number);
            var newRoom = new Room
            {
                Number = number,
                Category = category
            };
            Entity.Rooms.Add(newRoom);
            Publish(new RoomAdded
            {
                CruiseId = Entity.Id,
                Number = newRoom.Number,
                Category = newRoom.Category
            });
        }

        private void CheckForDuplicateRoomNumbers(int number)
        {
            if (Entity.Rooms.Any(room => room.Number == number))
            {
                throw new Exception("Duplicate room number");
            }
        }
    }
}
