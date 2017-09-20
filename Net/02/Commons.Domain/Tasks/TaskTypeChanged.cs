using Cqrs;
using System;

namespace Commons.Tasks
{
    public class TaskTypeChanged : IEvent
    {
        public TaskTypeChanged()
        {

        }

        public TaskTypeChanged(Guid id, string title)
        {
            Id = id;
            this.New = title;
        }

        public Guid Id { get; set; }
        public String Old { get; set; }
        public String New { get; set; }
    }
}
