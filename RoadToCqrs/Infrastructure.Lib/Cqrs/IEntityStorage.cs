using System;
using System.Collections.Generic;
using Infrastructure.Lib.ServiceBus;

namespace Infrastructure.Lib.Cqrs
{
    public interface IEntityStorage
    {


        void Save<T>(AggregateRoot<T> aggregate, int expectedVersion = -1) where T : IAggregateEntity;

        T GetById<T>(Guid id) where T : IAggregateRoot;
    }
}
