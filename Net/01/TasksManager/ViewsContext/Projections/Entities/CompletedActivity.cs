using System;

namespace TasksManager.ViewsContext.Projections.Entities
{
    public class CompletedActivity
    {
        public Guid Id { get; set; }
        public int Day { get; set; }
        public DateTime To { get; set; }
        public DateTime From { get; set; }
        public string Description { get; set; }
    }
}
