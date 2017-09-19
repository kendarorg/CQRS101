using Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Tasks
{
    public class TaskDescriptionChanged : IEvent
    {
        public TaskDescriptionChanged()
        {

        }

        public TaskDescriptionChanged(Guid id, string description)
        {
            Id = id;
            this.New = description;
        }

        public Guid Id { get; set; }
        public String Old { get; set; }
        public String New { get; set; }
    }
}
