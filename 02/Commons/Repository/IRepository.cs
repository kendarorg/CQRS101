using System.Collections.Generic;

namespace Commons
{
    public interface IRepository : IService
    {

    }

    public interface IRepository<T, TK> : IRepository where T : class
    {
        T Save(T entity);
        TK GetKey(T entity);
        void SetKey(T entity, TK key);
        bool Delete(T entity);
        bool Delete(TK entityKey);
        bool Update(T entity);
        IEnumerable<T> Find(IFilter filter = null);
        T GetById(TK key);
    }
}
