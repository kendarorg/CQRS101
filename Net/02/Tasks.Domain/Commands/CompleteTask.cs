using Cqrs;
using System;

namespace Tasks.Commands
{
    public class CompleteTask : ICommand
    {
        public Guid Id { get; set; }
    }
}
