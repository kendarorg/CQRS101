using Cqrs.Commons;
using System;

namespace TasksManager.SharedContext.Events
{
    public class ActivityCompleted : IEvent
    {
        public Guid Id { get; set; }
        public int Day { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Description { get; set; }
        public string TypeCode { get; set; }
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
    }
}
