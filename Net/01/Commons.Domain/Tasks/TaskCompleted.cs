using Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Tasks
{
    public class TaskCompleted : IEvent
    {
        public Guid Id { get; set; }
        public DateTime CompletionDate { get; set; }
    }
}
