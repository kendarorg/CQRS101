using Cqrs;
using System;

namespace Tasks.Commands
{
    public class CreateTask : ICommand
    {
        public DateTime? DueDate { get; set; }
        public Guid Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public int Priority { get; set; }
    }
}
