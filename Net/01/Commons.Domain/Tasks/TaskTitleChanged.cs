using Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Tasks
{
    public class TaskTitleChanged : IEvent
    {
        public TaskTitleChanged()
        {

        }

        public TaskTitleChanged(Guid id, string title)
        {
            Id = id;
            this.New = title;
        }

        public Guid Id { get; set; }
        public String Old { get; set; }
        public String New { get; set; }
    }
}
