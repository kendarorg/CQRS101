using System;
using System.Linq;
using Crud;

namespace InMemory.Crud
{
    public class InMemoryOptimisticRepository<T> : InMemoryRepository<T>,IOptimisticRepository<T> where T:IOptimisticEntity
    {
        private readonly object _lock = new object();

        public T GetByIdVersion(Guid id, long version)
        {
            return GetAll(a => a.Id == id && a.Version == version).FirstOrDefault();
        }

        public override Guid Save(T toUpdate)
        {
            lock (_lock)
            {
                if (!Items.ContainsKey(toUpdate.Id))
                {

                    toUpdate.Version = 0;
                    base.Save(toUpdate);
                }
                else
                {
                    var oldItemVersion = Items[toUpdate.Id].Version;
                    if (oldItemVersion != toUpdate.Version)
                    {
                        throw new OptimisticWriteException();
                    }
                    toUpdate.Version++;
                    base.Save(toUpdate);
                }
                return toUpdate.Id;
            }
        }
    }
}
