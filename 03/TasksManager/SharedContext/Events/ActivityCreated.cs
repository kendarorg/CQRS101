﻿using Cqrs.Commons;
using System;

namespace TasksManager.SharedContext.Events
{
    public class ActivityCreated : IEvent
    {
        public Guid CompanyId { get; set; }

        public Guid Id { get; set; }
        public int Day { get; set; }
        public DateTime From { get; set; }
        public string Description { get; set; }
        public string TypeCode { get; set; }
        public Guid UserId { get; set; }
    }
}
