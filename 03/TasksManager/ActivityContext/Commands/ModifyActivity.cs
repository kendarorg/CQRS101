using Cqrs.Commons;
using System;

namespace TasksManager.ActivityContext.Commands
{
    public class ModifyActivity : ICommand
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public int Day { get; set; }
        public string Description { get; set; }
        public string ActivityTypeCode { get; set; }
    }
}
