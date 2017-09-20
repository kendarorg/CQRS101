using Cqrs;
using System;

namespace Tasks.Commands
{
    public class ChangeTaskDescription : ICommand
    {
        public Guid Id { get; set; }
        public String Description { get; set; }
    }
}
