using Cqrs03.Test.Infrastructure;
using System;
using System.Collections.Generic;

namespace Cqrs01.Test.Infrastructure
{
    public class ItemToSend
    {
        public object Data { get; set; }
        public DateTime? DelaySend { get; set; }
    }
    public class AggregateRoot<T> where T: IAggregateEntity
    {
        protected AggregateRoot()
        {

        }
        public AggregateRoot(T entity)
        {
            Entity = entity;
        }
        private readonly List<ItemToSend> _unsentEvents = new List<ItemToSend>();

        public IEnumerable<ItemToSend> GetUnsentEvents()
        {
            return _unsentEvents;
        }

        public void ClearEvents()
        {
            _unsentEvents.Clear();
        }

        public T Entity { get; protected set; }

        protected void Publish(object @event,DateTime? delayToSend = null)
        {
            _unsentEvents.Add(new ItemToSend { Data = @event, DelaySend=delayToSend });
        }
    }
}
