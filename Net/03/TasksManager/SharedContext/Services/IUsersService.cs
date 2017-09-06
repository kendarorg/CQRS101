using Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManager.SharedContext.VOs
{
    public interface IUsersService : IService
    {
        Guid GetUserCompany(Guid userId);
    }
}
