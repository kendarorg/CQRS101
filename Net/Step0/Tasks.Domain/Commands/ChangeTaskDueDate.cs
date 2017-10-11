using Cqrs;
using System;

namespace Tasks.Commands
{
    public class ChangeTaskDueDate : ICommand
    {
        public Guid Id { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
