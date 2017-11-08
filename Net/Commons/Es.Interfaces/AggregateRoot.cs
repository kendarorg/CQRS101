using Cqrs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Es.Interfaces
{
    public abstract class AggregateRoot
    {
        private static ConcurrentDictionary<Type, ConcurrentDictionary<Type, Action<Object>>> applyFunctions = new ConcurrentDictionary<Type, ConcurrentDictionary<Type, Action<object>>>();

        protected void RegisterApply(Action<Object> applyFunction, Type messageType)
        {
            applyFunctions[this.GetType()] = new ConcurrentDictionary<Type, Action<Object>>();
            applyFunctions[this.GetType()][messageType] = applyFunction;
        }

        private void DoApply(IEvent evt)
        {
            ConcurrentDictionary<Type, Action<Object>> aggregateApplier = applyFunctions[this.GetType()];
            Action<Object> functionApplier = aggregateApplier[evt.GetType()];
            functionApplier.Invoke(evt);
        }

        protected AggregateRoot()
        {
            InitializeApply();
        }

        protected abstract void InitializeApply();

        private List<IEvent> changes = new List<IEvent>();
        public Guid Id { get; protected set; }

        public List<IEvent> GetUncommittedChanges()
        {
            return changes;
        }

        public void MarkChangesAsCommitted()
        {
            changes.Clear();
        }

        public void LoadsFromHistory(List<IEvent> history)
        {
            foreach (IEvent e in history)
            {
                ApplyChange(e, false);
            }
        }

        private void ApplyChange(IEvent evt, bool isNew = true)
        {
            DoApply(evt);
            if (isNew)
            {
                changes.Add(evt);
            }
        }
    }
}
