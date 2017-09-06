using System;
using Cqrs.Commons;

namespace Cqrs.LogContext.Commands
{
    public class CreateActivity : ICommand
    {
        public DateTime? From { get; set; }
        public string Description { get; set; }
    }
}
