using System;

namespace Bus
{
    public interface IBus
    {
        void Send(object message,DateTime? delayed=null);
        void RegisterTopic<T>(Action<T> handler);
        void RegisterQueue<T>(Action<T> handler);
        void AddListener(Action<object> listener);
    }
}
