using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Commons.Implementation
{
    public abstract class MockRepository<T, TK> : IRepository<T, TK> where T : class
    {
        protected abstract ConcurrentDictionary<string, T> GetStorage();
        public abstract TK GetKey(T entity);
        public abstract void SetKey(T entity, TK key);
        protected abstract TK InitializeKey(TK key);

        private T Save(T item, TK key)
        {
            key = InitializeKey(key);
            var stringKey = key.ToString();
            var storage = GetStorage();
            item = RefreshSingle(item);
            storage[stringKey] = item;
            return item;
        }

        protected virtual T RefreshSingle(T item)
        {
            return item;
        }

        public T Save(T entity)
        {
            return Save(entity, GetKey(entity));
        }

        public bool Delete(T entity)
        {
            return Delete(GetKey(entity));
        }

        public bool Delete(TK entityKey)
        {
            var stringKey = entityKey.ToString();
            var storage = GetStorage();
            if (storage.ContainsKey(stringKey))
            {
                T result;
                return storage.TryRemove(stringKey, out result);
            }
            return false;
        }

        public bool Update(T entity)
        {
            var toUpdate = GetById(GetKey(entity));
            if (toUpdate == default(T)) return false;
            Save(entity);
            return true;
        }

        public IEnumerable<T> Find(IFilter filter = null)
        {
            var result = GetStorage().Values;

            foreach (var item in Filter(result, filter))
            {
                yield return RefreshSingle(item);
            }
        }

        protected virtual IEnumerable<T> Filter(IEnumerable<T> result, IFilter filter)
        {
            return result;
        }

        public T GetById(TK key)
        {
            var stringKey = key.ToString();
            var storage = GetStorage();
            if (storage.ContainsKey(stringKey))
            {
                return storage[stringKey];
            }
            return default(T);
        }
    }
}
