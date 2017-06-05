using Cqrs.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Cqrs.Commons.Infrastracture
{
    public class MockBus : IBus
    {
        private readonly IList<ICommandHandler> _commandHandlers;
        private readonly IList<IEventHandler> _eventHandlers;

        public MockBus(
            IEnumerable<ICommandHandler> commandHandlers,
            IEnumerable<IEventHandler> eventHandlers)
        {
            _commandHandlers = commandHandlers.ToList();
            _eventHandlers = eventHandlers.ToList();
        }

        public Task SendAsync(IMessage message)
        {
            return Task.Run(() => SendSync(message));
        }

        public void SendSync(IMessage message)
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;

            var pars = new object[] { message };
            var types = new[] { message.GetType() };
            if (RunCommandHandlers(bindingFlags, types, pars)) return;
            RunEventHandlers(bindingFlags, types, pars);
        }

        private bool RunCommandHandlers(BindingFlags bindingFlags, Type[] types, object[] pars)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < _commandHandlers.Count; i++)
            {
                var cmdHandler = _commandHandlers[i];
                var method = cmdHandler.GetType().
                    GetMethod("Handle", bindingFlags, null, types, null);
                if (method != null)
                {
                    method.Invoke(cmdHandler, pars);
                    //Commands are executed once only!
                    return true;
                }
            }
            return false;
        }

        private void RunEventHandlers(BindingFlags bindingFlags, Type[] types, object[] pars)
        {
            var exceptions = new List<Exception>();

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < _eventHandlers.Count; i++)
            {
                var evtHandler = _eventHandlers[i];
                var method = evtHandler.GetType().
                    GetMethod("Handle", bindingFlags, null, types, null);
                if (method != null)
                {
                    try
                    {
                        method.Invoke(evtHandler, pars);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}
