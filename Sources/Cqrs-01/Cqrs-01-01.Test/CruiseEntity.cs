using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs_01_01.Test
{
    public class CruiseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public CruiseEntity(Guid id,string name)
        {
            Id = id;
            Name = name;
        }
    }
}
