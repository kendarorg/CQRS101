using Cqrs.Commons;
using System;

namespace TasksManager.UsersContext.Commands
{
    public class ChangeUserPassword:ICommand
    {
        public Guid UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
