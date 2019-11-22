using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Lib.Cqrs
{
    public interface IEvent
    {
        int Version { get; set; }
    }
}
