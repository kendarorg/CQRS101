﻿using System;
using System.Collections.Generic;

namespace Infrastructure.Lib.Cqrs
{
    public class ItemToSend
    {
        public object Data { get; set; }
        public DateTime? DelaySend { get; set; }
    }


    public class AggregateRoot<T>: IAggregateRoot where T : IAggregateEntity
    {
        private readonly List<ItemToSend> _unsentEvents = new List<ItemToSend>();

        protected AggregateRoot()
        {

        }
        public AggregateRoot(T entity)
        {
            Entity = entity;
        }

        public Guid Id { get { return Entity.Id; } }

        public IEnumerable<ItemToSend> GetUnsentEvents()
        {
            return _unsentEvents;
        }

        public void ClearEvents()
        {
            _unsentEvents.Clear();
        }

        public T Entity { get; protected set; }

        protected void Publish(object @event, DateTime? delayToSend = null)
        {
            _unsentEvents.Add(new ItemToSend { Data = @event, DelaySend = delayToSend });
        }
    }
}
