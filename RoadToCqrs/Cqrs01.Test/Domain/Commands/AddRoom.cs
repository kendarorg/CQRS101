﻿using System;

namespace Cqrs01.Test.Domain.Commands
{
    public class AddRoom
    {
        public AddRoom(Guid cruiseId,int number,int category)
        {
            CruiseId = cruiseId;
            Number = number;
            Category = category;
        }
        public Guid CruiseId { get; set; }
        public int Number { get; set; }
        public int Category { get; set; }
    }
}
