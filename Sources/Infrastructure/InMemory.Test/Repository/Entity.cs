using Crud;
using System;

namespace InMemory.Repository
{
    public class Entity : IEntity
    {
        public Guid Id { get; set; }
        public string Data { get; set; }
    }
}
