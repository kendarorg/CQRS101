using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Commands
{
    public class ChangeTaskDueDate
    {
        public Guid Id { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
