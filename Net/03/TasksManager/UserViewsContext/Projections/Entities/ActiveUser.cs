using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManager.UserViewsContext.Projections.Entities
{
    public class ActiveUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}
