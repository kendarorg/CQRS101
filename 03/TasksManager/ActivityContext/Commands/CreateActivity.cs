using System;
using Cqrs.Commons;

namespace Cqrs.LogContext.Commands
{
    public class CreateActivity : ICommand
    {
        public Guid UserId { get; set; }
        public DateTime? From { get; set; }
        public string Description { get; set; }
        public string ActivityTypeCode { get; set; }
    }
}
