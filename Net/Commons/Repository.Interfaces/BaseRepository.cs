using Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Interfaces
{
    public abstract class BaseRepository<T> : IRepository<T>
    {
        protected IRepositoryHelper helper;

        protected BaseRepository(IRepositoryHelper helper)
        {
            Type clazz = typeof(T);
            this.helper = helper.Create(clazz);
        }

        public void Delete(Guid id)
        {
            helper.Delete(id);
        }

        public List<T> GetAll()
        {
            return helper.GetAll().Select(a =>(T)a).ToList();
        }

        public T GetById(Guid id)
        {
            return (T)helper.GetById(id);
        }

        public abstract T Save(T item);
    }
}
