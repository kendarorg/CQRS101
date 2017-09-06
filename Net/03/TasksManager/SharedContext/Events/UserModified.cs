using Cqrs.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManager.SharedContext.Events
{
    public class UserModified : IEvent
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid Id { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsEnabled { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
