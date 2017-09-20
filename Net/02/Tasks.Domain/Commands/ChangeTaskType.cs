using Cqrs;
using System;

namespace Tasks.Commands
{
    public class ChangeTaskType : ICommand
    {
        public Guid Id { get; set; }
        public string TypeCode { get; set; }
    }
}
