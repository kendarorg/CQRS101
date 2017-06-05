using System;

namespace TasksManager.ViewsContext.Projections.Entities
{
    public class ActivitySummaryKey
    {
        public int Day { get; set; }
        public string TypeCode { get; set; }
        public override string ToString()
        {
            return string.Format("{0}/{1}", Day, TypeCode);
        }
    }

    public class ActivitySummary
    {
        public int Day { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
