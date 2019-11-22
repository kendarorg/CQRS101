namespace Crud
{
    public interface IOptimisticEntity:IEntity
    {
        long Version { get; set; }
    }
}
