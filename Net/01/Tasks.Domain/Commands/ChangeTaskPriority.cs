using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Commands
{
    public class ChangeTaskPriority
    {
        public Guid Id { get; set; }
        public int Priority { get; internal set; }
    }
}
