using Crud;

namespace InMemory.Repository
{
    public class OptimisticEntity :Entity, IOptimisticEntity
    {
        public long Version { get; set; }
    }
}
