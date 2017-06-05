using System;
using Cqrs.Commons;

namespace Cqrs.LogContext.Commands
{
    public class CompleteActivity : ICommand
    {
        public Guid UserId { get; set; }
        public int Day { get; set; }
        public Guid Id { get; set; }
        public DateTime To { get; set; }
    }
}