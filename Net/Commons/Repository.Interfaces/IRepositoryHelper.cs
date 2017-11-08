using System;
using System.Collections.Generic;
using System.Text;
using Utils;

namespace Repository.Interfaces
{
    public interface IRepositoryHelper : ISingleton
    {

        IRepositoryHelper Create(Type clazz);

        String Name { get; }

        Object GetById(Guid id);

        List<Object> GetAll();

        Object Save(Object item, Action<Object, Guid> idSetter, Func<Object, Guid> idGetter);

        void Delete(Guid id);
    }
}
