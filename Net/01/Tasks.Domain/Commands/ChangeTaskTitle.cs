using Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Commands
{
    public class ChangeTaskTitle:IMessage
    {
        public Guid Id { get; set; }
        public string Title { get; internal set; }
    }
}
