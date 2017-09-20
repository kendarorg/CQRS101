using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Repositories
{
    public class TaskDao
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int Priority { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public String TypeCode { get; set; }
    }
}
