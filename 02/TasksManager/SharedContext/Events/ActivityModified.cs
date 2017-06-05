using Cqrs.Commons;
using System;

namespace TasksManager.SharedContext.Events
{
    public class ActivityModified : IEvent
    {
        public Guid Id { get; set; }
        public int Day { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
    }
}
