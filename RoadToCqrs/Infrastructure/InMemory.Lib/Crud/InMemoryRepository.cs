using Crud;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable InconsistentlySynchronizedField
// ReSharper disable RedundantLambdaSignatureParentheses
namespace InMemory.Crud
{
    public class InMemoryRepository<T> : IRepository<T> where T : IEntity
    {
        internal readonly IDictionary<Guid, T> Items;
        private readonly object _lock = new object();
        private readonly bool _hasLogicalDelete;

        private static bool IsLogicallyDeleted(object toCheck)
        {
            var ld = toCheck as ILogicalDeleteEntity;
            return ld != null && ld.IsDeleted;
        }

        protected T Clone(T input)
        {
            if (input == null)
            {
                return default;
            }

            var tmp = JsonConvert.SerializeObject(input);
            return JsonConvert.DeserializeObject<T>(tmp);
        }

        public InMemoryRepository()
        {
            _hasLogicalDelete = typeof(ILogicalDeleteEntity).IsAssignableFrom(typeof(T));
            Items = new Dictionary<Guid, T>();
        }

        public void Delete(Guid id)
        {
            // ReSharper disable once InvertIf
            if (Items.ContainsKey(id))
            {
                var item = GetById(id);
                if (_hasLogicalDelete)
                {
                    ((ILogicalDeleteEntity) item).IsDeleted = true;
                    Save(item);
                }else{
                    Items.Remove(id);
                }
            }
        }

        public IEnumerable<T> GetAll(Func<T, bool> query = null)
        {
            return Items.Values
                .Where((a) => query == null || query(a) && !IsLogicallyDeleted(a))
                .Select(Clone);
        }

        public T GetById(Guid id)
        {
            return Clone(GetAll((a) => a.Id == id).FirstOrDefault());
        }

        public virtual Guid Save(T toUpdate)
        {
            lock (_lock)
            {
                if (toUpdate.Id == Guid.Empty)
                {
                    toUpdate.Id = Guid.NewGuid();
                }

                if (!Items.ContainsKey(toUpdate.Id))
                {
                    Items.Add(toUpdate.Id, Clone(toUpdate));
                }
                else
                {
                    Items[toUpdate.Id] = Clone(toUpdate);
                }
            }

            return toUpdate.Id;
        }
    }
}