using System.Collections.Generic;

namespace Cqrs01.Test.Infrastructure
{
    public class AggregateRoot<T>
    {

        protected AggregateRoot()
        {

        }
        public AggregateRoot(T entity)
        {
            Entity = entity;
        }
        private readonly List<object> _unsentEvents = new List<object>();

        public IEnumerable<object> GetUnsentEvents()
        {
            return _unsentEvents;
        }

        public void ClearEvents()
        {
            _unsentEvents.Clear();
        }

        public T Entity { get; protected set; }

        protected void Publish(object @event)
        {
            _unsentEvents.Add(@event);
        }
    }
}
