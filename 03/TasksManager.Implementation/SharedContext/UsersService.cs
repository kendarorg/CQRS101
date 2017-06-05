using Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManager.Implementation.UsersContext.Repositories.Entities;
using TasksManager.SharedContext.VOs;

namespace TasksManager.Implementation.SharedContext
{
    public class UsersService : IUsersService
    {
        private IRepository<User, Guid> _users;

        public UsersService(IRepository<User, Guid> users)
        {
            _users = users;
        }

        public Guid GetUserCompany(Guid userId)
        {
            var user = _users.GetById(userId);
            return user.CompanyId;
        }
    }
}
