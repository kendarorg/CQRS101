using System;

namespace Infrastructure.Lib.Cqrs
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
