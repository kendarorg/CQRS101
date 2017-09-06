using Commons;

namespace Cqrs.Commons
{
    public interface ICommandHandler<in T> : ICommandHandler where T : ICommand
    {
        void Handle(T message);
    }

    public interface ICommandHandler : IService
    {
    }
}