using Crud;
using System;

namespace Cruise
{
    public class CruiseProjectionEntity:IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
