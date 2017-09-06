using Cqrs.Commons;
using System;

namespace TasksManager.SharedContext.Events
{
    public class CompanyCreated:IEvent
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public bool IsEnabled { get; set; }

    }
}
