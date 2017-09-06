using Commons;
using Commons.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TasksManager.UserViewsContext.Projections.Entities;
using TasksManager.ViewsContext.Projections.Entities;

namespace Cqrs.Api.Controllers
{
    [RoutePrefix("api/users")]
    public class UserViewsController : ApiController
    {
        private IRepository<ActiveCompany, Guid> _activeCompanies;
        private IRepository<ActiveUser, Guid> _activeUsers;
        private IRepository<InactiveCompany, Guid> _inactiveCompanies;
        private IRepository<InactiveUser, Guid> _inctiveUsers;

        public UserViewsController(
            IRepository<ActiveUser, Guid> activeUsers,
            IRepository<InactiveUser, Guid> inctiveUsers,
            IRepository<ActiveCompany, Guid> activeCompanies,
            IRepository<InactiveCompany, Guid> inactiveCompanies)
        {
            _activeUsers = activeUsers;
            _inctiveUsers = inctiveUsers;
            _activeCompanies = activeCompanies;
            _inactiveCompanies = inactiveCompanies;
        }

        [HttpPost]
        [Route("users/active")]
        public IEnumerable<ActiveUser> GetActiveUsers(Filter filter)
        {
            return _activeUsers.Find(filter);
        }

        [HttpPost]
        [Route("users/inactive")]
        public IEnumerable<InactiveUser> GetInactiveUsers(Filter filter)
        {
            return _inctiveUsers.Find(filter);
        }

        [HttpPost]
        [Route("companies/active")]
        public IEnumerable<ActiveCompany> GetActiveCompanies(Filter filter)
        {
            return _activeCompanies.Find(filter);
        }

        [HttpPost]
        [Route("companies/inactive")]
        public IEnumerable<InactiveCompany> GetInactiveCompanies(Filter filter)
        {
            return _inactiveCompanies.Find(filter);
        }
    }
}