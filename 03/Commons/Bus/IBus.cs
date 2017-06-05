using System.Threading.Tasks;
using Commons;

namespace Cqrs.Commons
{
    public interface IBus : IService
    {
        Task SendAsync(IMessage message);
        void SendSync(IMessage message);
    }
}