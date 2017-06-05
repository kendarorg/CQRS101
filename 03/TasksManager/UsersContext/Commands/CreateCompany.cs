using Cqrs.Commons;

namespace TasksManager.UsersContext.Commands
{
    public class CreateCompany :ICommand
    {
        public string Name { get; set; }
    }
}
