using Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Tasks
{
    public class TaskCreated : IEvent
    {
        public TaskCreated()
        {

        }
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public TaskDueDateChanged DueDateSet { get; set; }
        public TaskPriorityChanged PrioritySet { get; set; }
        public TaskDescriptionChanged DescriptionSet { get; set; }
        public TaskTitleChanged TitleSet { get; set; }
    }
}
