using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManager.Implementation.ActivityContext.Repositories.Entities
{
    public class ActivityDay
    {
        public ActivityDay()
        {
            Activities = new List<Activity>();
        }

        public int Day { get; set; } //YYYYMMGG
        public List<Activity> Activities { get; set; }
        public Guid CompanyId { get; internal set; }
        public Guid UserId { get; internal set; }
    }
}
