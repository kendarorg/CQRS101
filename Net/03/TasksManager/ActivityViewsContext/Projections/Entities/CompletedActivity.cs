using System;

namespace TasksManager.ViewsContext.Projections.Entities
{
    public class CompletedActivity
    {
        public Guid Id { get; set; }
        public int Day { get; set; }
        public DateTime To { get; set; }
        public DateTime From { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
    }
}
