using Infrastructure.Lib.Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Lib.ServiceBus
{
    public class MessageHandlersFinder
    {
        private static MessageHandlersFinder _messageHandlersFinder;
        private readonly static object _lock = new object();

        public static void SetMessageHandlersFinder(MessageHandlersFinder messageHAndlersFinder)
        {
            _messageHandlersFinder = messageHAndlersFinder;
        }
        public static MessageHandlersFinder GetMessageHandlersFinder()
        {
            if (_messageHandlersFinder != null)
            {
                return _messageHandlersFinder;
            }
            lock (_lock)
            {
                _messageHandlersFinder = new MessageHandlersFinder();
                _messageHandlersFinder.Initialize();
            }
            return _messageHandlersFinder;
        }

        private readonly Dictionary<Type, Dictionary<Type, MethodInfo>> _applyList = new Dictionary<Type, Dictionary<Type, MethodInfo>>();

        private void Initialize()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies().Where(asm => !asm.IsDynamic))
            {
                try
                {
                    foreach (var type in asm.GetTypes())
                    {
                        if (typeof(IMessageHandler).IsAssignableFrom(type))
                        {
                            _applyList.Add(type, new Dictionary<Type, MethodInfo>());
                            foreach (var @interface in type.GetInterfaces()
                                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMessageHandler<>)))
                            {
                                var handlerMessage = @interface.GetGenericArguments()[0];
                                if (typeof(ICommand).IsAssignableFrom(handlerMessage))
                                {

                                }
                                else if (typeof(IEvent).IsAssignableFrom(handlerMessage))
                                {

                                }
                            }
                            foreach (var methodInfo in type
                                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                                .Where(m => m.Name == "Apply" && m.GetParameters().Count() == 1))
                            {
                                var methodParamType = methodInfo.GetParameters()[0].ParameterType;
                                if (typeof(IEvent).IsAssignableFrom(methodParamType))
                                {
                                    if (!_applyList.ContainsKey(type))
                                    {
                                        _applyList[type] = new Dictionary<Type, MethodInfo>();
                                    }
                                    _applyList[type][methodParamType] = methodInfo;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    //NOP
                }
            }
        }

        public virtual void ApplyEvent(AggregateRoot root, IEvent @event)
        {
            if (_applyList.ContainsKey(root.GetType()) && _applyList[root.GetType()].ContainsKey(@event.GetType()))
            {
                _applyList[root.GetType()][@event.GetType()].Invoke(root, new object[] { @event });
            }
        }
    }
}
