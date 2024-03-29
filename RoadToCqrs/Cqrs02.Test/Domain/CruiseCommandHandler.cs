﻿using Cqrs01.Test.Domain.Commands;
using Cqrs01.Test.Infrastructure;
using Cqrs02.Test.Infrastructure;

namespace Cqrs01.Test.Domain
{
    public class CruiseCommandHandler
    {
        private readonly EntityStorage _entityStorage;

        public CruiseCommandHandler(Bus bus,EntityStorage entityStorage)
        {
            _entityStorage = entityStorage;
            bus.RegisterQueue<CreateCruise>(Handle);
            bus.RegisterQueue<AddRoom>(Handle);
        }
        public void Handle(CreateCruise command)
        {
            var aggregate = new CruiseAggregateRoot(command.CruiseId, command.Name);
            _entityStorage.Save(command.CruiseId,aggregate);
        }
        public void Handle(AddRoom command)
        {
            var entity = _entityStorage.GetById<CruiseEntity>(command.CruiseId);
            var aggregate = new CruiseAggregateRoot(entity);
            aggregate.AddRoom(command.Number, command.Category);
            _entityStorage.Save(command.CruiseId, aggregate);
        }
    }
}
