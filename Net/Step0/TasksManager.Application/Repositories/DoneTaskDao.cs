using System;

namespace TasksManager.Repositories
{
    public class DoneTaskDao
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Title { get; set; }
        public int Priority { get; set; }
    }
}
