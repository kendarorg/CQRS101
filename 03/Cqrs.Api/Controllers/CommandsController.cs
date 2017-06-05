using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Cqrs.Commons;
using Newtonsoft.Json;

namespace Cqrs.Api.Controllers
{
    [RoutePrefix("api/command")]
    public class CommandsController : ApiController
    {
        private readonly IBus _bus;
        private static readonly ConcurrentDictionary<string, Type> Types;

        static CommandsController()
        {
            Types = new ConcurrentDictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);
        }

        public CommandsController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        [Route("{commandName}")]
        public void Send(string commandName)
        {
            var commandTask = Request.Content.ReadAsStringAsync();
            commandTask.Wait();
            var type = Types.GetOrAdd(commandName, LoadType);
            var command = (IMessage)JsonConvert.DeserializeObject(commandTask.Result, type);
            _bus.SendSync(command);
        }

        #region Utilities

        private static Type LoadType(string commandName)
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic))
            {
                var type = LoadType(commandName, asm);
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }

        private static Type LoadType(string commandName, Assembly asm)
        {
            try
            {
                return asm.GetTypes()
                    .FirstOrDefault(t => !t.IsAbstract && !t.IsEnum && !t.IsInterface && t.IsClass &&
                        string.Compare(t.Name, commandName,
                            StringComparison.InvariantCultureIgnoreCase) == 0);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion Utilities
    }
}