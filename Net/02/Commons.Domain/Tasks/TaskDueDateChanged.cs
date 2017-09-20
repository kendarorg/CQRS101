using Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Tasks
{
    public class TaskDueDateChanged : IEvent
    {
        public TaskDueDateChanged()
        {

        }

        public TaskDueDateChanged(Guid id, DateTime? dueDate)
        {
            Id = id;
            this.New = dueDate;
        }

        public Guid Id { get; set; }
        public DateTime? Old { get; set; }
        public DateTime? New { get; set; }
    }
}
