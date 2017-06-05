using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using Commons.Repository;
using Commons.Repositories;
using System.ComponentModel;
using DynamicExpresso;
using TB.ComponentModel;

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

        public IEnumerable<T> Find(Filter filter = null)
        {
            var result = GetStorage().Values;

            foreach (var item in Filter(result, filter))
            {
                yield return RefreshSingle(item);
            }
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

        protected virtual IEnumerable<T> Filter(IEnumerable<T> result, Filter filter)
        {
            var interpreter = new Interpreter();
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).
                Where(p => p.CanRead && p.CanWrite).ToList();

            var converted = ConvertToMockFilter(interpreter, filter, properties);
            foreach (var item in result)
            {
                if (converted.Match(item))
                {
                    yield return item;
                }
            }
        }

        private MockFilter ConvertToMockFilter(Interpreter interpreter, Filter filter, List<PropertyInfo> properties)
        {
            if (filter.Type != FilterType.And && filter.Type != FilterType.Or)
            {
                var property = properties.First(f => f.Name == filter.Field);
                
                object realValue  = UniversalTypeConverter.Convert(filter.Value,property.PropertyType);
                var expression = string.Format("target.{0} {1} toCompare",
                    filter.Field, GetCompareString(filter.Type));
                var lambda = interpreter.Parse(expression,
                    new Parameter("target", typeof(T)), new Parameter("toCompare", property.PropertyType));
                return new MockFilter
                {
                    Lambda = lambda,
                    Value = realValue
                };
            }
            else
            {
                return new MockFilter
                {
                    Type = filter.Type,
                    Conditions = filter.Conditions.
                        Select(c => ConvertToMockFilter(interpreter, c, properties)).ToList()
                };
            }
        }

        private string GetCompareString(FilterType type)
        {
            switch (type)
            {
                case (FilterType.Contains):
                    throw new NotSupportedException();
                case (FilterType.Greater):
                    return ">";

                case (FilterType.GreaterEqual):
                    return ">=";
                case (FilterType.Lower):
                    return "<";
                case (FilterType.LowerEqual):
                    return "<=";
                case (FilterType.Equal):
                default:
                    return "==";

            }
        }
    }
}
