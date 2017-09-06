using Cqrs.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManager.SharedContext.Events
{
    public class CompanyModified : IEvent
    {
        public string CompanyName { get; set; }
        public Guid Id { get; set; }
        public bool IsEnabled { get; set; }
    }
}
