using System;
using System.Collections.Generic;
using Utils;

namespace Cqrs
{
    public interface IRepository<T>:ISingleton
    {
        T GetById(Guid id);
        List<T> GetAll();
        T Save(T item);
        void Delete(Guid id);
    }
}
