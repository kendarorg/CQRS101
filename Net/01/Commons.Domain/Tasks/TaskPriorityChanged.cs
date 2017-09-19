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

        public TaskPriorityChanged()
        {

        }

        public TaskPriorityChanged(Guid id, int priority)
        {
            Id = id;
            this.New = priority;
        }

        public Guid Id { get; set; }
        public int Old { get; set; }
        public int New { get; set; }
    }
}
