using System;

namespace TasksManager.Implementation.ActivityContext.Repositories.Entities
{
    public class Activity
    {
        public Guid Id { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        public string Description { get; set; }
    }
}
