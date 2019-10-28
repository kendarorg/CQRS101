using System;

namespace Crud
{
    public interface IOptimisticRepository<T> : 
        IRepository<T> where T:IOptimisticEntity
    {
        T GetByIdVersion(Guid id, long version);
    }
}
