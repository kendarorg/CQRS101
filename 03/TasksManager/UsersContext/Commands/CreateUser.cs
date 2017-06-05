using Cqrs.Commons;
using System;

namespace TasksManager.UsersContext.Commands
{
    public class CreateUser : ICommand
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
