using Cqrs;
using System;

namespace Tasks.Commands
{
    public class ChangeTaskPriority : ICommand
    {
        public Guid Id { get; set; }
        public int Priority { get; set; }
    }
}
