using System;

namespace TasksManager.ViewsContext.Projections.Entities
{
    public class NotCompletedActivity
    {
        public Guid Id { get; set; }
        public int Day { get; set; }
        public DateTime From { get; set; }
        public string Description { get; set; }
    }
}
