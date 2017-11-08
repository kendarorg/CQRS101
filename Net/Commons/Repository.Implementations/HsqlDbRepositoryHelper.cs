using Repository.Interfaces;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Repository.Implementations
{
    public class HsqlDbRepositoryHelper : IRepositoryHelper
    {
        private static ConcurrentDictionary<Type, ConcurrentDictionary<Guid, Object>> storage = new ConcurrentDictionary<Type, ConcurrentDictionary<Guid, object>>();
        private Type clazz;
        
        public HsqlDbRepositoryHelper()
        {
            throw new NotImplementedException();
        }

        public IRepositoryHelper Create(Type clazz)
        {
            storage[clazz] = new ConcurrentDictionary<Guid, Object>();
            HsqlDbRepositoryHelper helper = new HsqlDbRepositoryHelper();
            helper.clazz = clazz;
            helper.Name = clazz.Name;
            return helper;
        }


        public String Name { get; private set; }


        public Object GetById(Guid id)
        {
            ConcurrentDictionary<Guid, Object> data = storage[clazz];
            if (!data.ContainsKey(id)) return null;
            return data[id];
        }


        public List<Object> GetAll()
        {
            ConcurrentDictionary<Guid, Object> data = storage[clazz];
            return data.Values.ToList();
        }


        public Object Save(Object item, Action<Object, Guid> idSetter, Func<Object, Guid> idGetter)
        {
            Guid id = idGetter.Invoke(item);
            if (id == null)
            {
                id = Guid.NewGuid();
                idSetter.Invoke(item, id);
            }
            ConcurrentDictionary<Guid, Object> data = storage[clazz];
            data[id] = item;
            return item;
        }


        public void Delete(Guid id)
        {
            ConcurrentDictionary<Guid, Object> data = storage[clazz];
            object value;
            data.TryRemove(id, out value);
        }
    }
}
