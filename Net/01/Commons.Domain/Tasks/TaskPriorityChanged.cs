using Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Tasks
{
    public class TaskPriorityChanged : IEvent
    {
        public Guid Id { get; set; }
        public int Old { get; set; }
        public int New { get; set; }
    }
}
