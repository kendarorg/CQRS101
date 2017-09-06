using Commons;

namespace Cqrs.Commons
{
    public interface IEventHandler<in T> : IEventHandler where T : IEvent
    {
        void Handle(T message);
    }

    public interface IEventHandler : IService
    {
    }
}