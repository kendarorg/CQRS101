
using System;

namespace TasksManager.Implementation.UsersContext.Repositories.Entities
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}
