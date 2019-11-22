using System;
using System.Collections.Generic;

namespace Crud
{
    public interface IRepository<T> where T : IEntity
    {
        void Delete(Guid id);
        Guid Save(T toUpdate);
        IEnumerable<T> GetAll(Func<T, bool> query = null);
        T GetById(Guid id);
    }
}
