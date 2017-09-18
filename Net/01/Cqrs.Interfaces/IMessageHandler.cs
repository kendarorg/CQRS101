using Utils;

namespace Cqrs
{
    public interface IMessageHandler:ISingleton
    {
        void Register(IBus bus);
    }
}
