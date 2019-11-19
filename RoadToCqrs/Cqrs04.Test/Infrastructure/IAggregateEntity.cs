namespace Cqrs03.Test.Infrastructure
{
    public interface IAggregateEntity
    {
        int Version { get; set; }
    }
}
