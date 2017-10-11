using Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Commands
{
    public class ChangeTaskTitle : ICommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}
