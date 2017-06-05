using Cqrs.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManager.SharedContext.Events
{
    public class UserPasswordModified : IEvent
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid Id { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}
