using System;

namespace TasksManager.Implementation.UsersContext.Repositories.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsEnabled { get; internal set; }
    }
}
