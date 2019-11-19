using Cruise.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs_01_01.Test
{
    public class CruiseAggregateRoot
    {
        public List LastEvent { get; protected set; }
        public CruiseEntity Entity { get; protected set; }

        public CruiseAggregateRoot()
        {

        }

        public CruiseAggregateRoot(Guid id,string name)
        {
            Entity = new CruiseEntity(id,name);
            LastEvent = new CruiseCreated(name, id);
        }

        public void CreateRoom(int number,int category,int expectedVersion)
        {

        }
    }
}
