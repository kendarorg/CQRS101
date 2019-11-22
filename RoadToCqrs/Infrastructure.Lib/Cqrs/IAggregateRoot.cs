using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Lib.Cqrs
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
    }
}
