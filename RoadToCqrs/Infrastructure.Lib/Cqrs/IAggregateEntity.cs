using System;

namespace Infrastructure.Lib.Cqrs
{
    public interface IAggregateEntity
    {
        int Version { get; set; }
        Guid Id { get; set; }
    }
}
