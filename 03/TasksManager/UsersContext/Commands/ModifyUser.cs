using Cqrs.Commons;
using System;

namespace TasksManager.UsersContext.Commands
{
    public class ModifyUser : ICommand
    {
        public Guid UserId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsEnabled { get; set; }
    }
}
