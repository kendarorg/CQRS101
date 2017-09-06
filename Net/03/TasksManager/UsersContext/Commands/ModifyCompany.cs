using Cqrs.Commons;
using System;

namespace TasksManager.UsersContext.Commands
{
    public class ModifyCompany:ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}
