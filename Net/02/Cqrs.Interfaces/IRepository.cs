using System;
using System.Collections.Generic;
using Utils;

namespace Cqrs
{
    public interface IRepository<T,K>:ISingleton
    {
        T GetById(K id);
        List<T> GetAll();
        T Save(T item);
        void Delete(K id);
    }
}
