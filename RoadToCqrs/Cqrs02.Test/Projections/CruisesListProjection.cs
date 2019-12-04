using Cqrs01.Test.Domain.Events;
using Cqrs02.Test.Infrastructure;
using System;
using System.Collections.Generic;

namespace Cqrs02.Test.Projections
{
    public class CruisesListProjection
    {
        private Dictionary<Guid, CruisesListEntity> _data = new Dictionary<Guid, CruisesListEntity>();

        public CruisesListProjection(Bus bus)
        {
            bus.RegisterTopic<CruiseCreated>(Handle);
            bus.RegisterTopic<RoomAdded>(Handle);
        }

        public CruisesListEntity GetById(Guid id)
        {
            return _data[id];
        }

        private void Handle(RoomAdded @event)
        {
            var entity = _data[@event.CruiseId];
            switch (@event.Category)
            {
                case (1):
                    entity.FirstClassRooms++;
                    break;
                case (2):
                    entity.SecondClassRooms++;
                    break;
                case (3):
                    entity.ThirdClassRooms++;
                    break;

            }
        }

        private void Handle(CruiseCreated @event)
        {
            _data[@event.CruiseId] = new CruisesListEntity(@event.CruiseId,@event.Name);
        }
    }
}
