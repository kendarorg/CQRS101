using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManager.Repositories
{
    public class TaskTypeStatDao
    {
        public String TypeCode { get; set; }
        public int Completed { get; set; }
        public int Running { get; set; }
    }
}
