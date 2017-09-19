using Cqrs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace TasksManager.Web.Controllers
{
    [RoutePrefix("api/commands")]
    public class CommandsController : ApiController
    {
        private static Dictionary<string, Type> _types;
        private IBus _bus;

        public CommandsController(IBus bus)
        {
            _bus = bus;
        }

        static CommandsController()
        {
            _types = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);
            var type = typeof(ICommand);
            foreach (var item in AppDomain.CurrentDomain.GetAssemblies()
                .Where(s => !s.IsDynamic)
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsAbstract && !p.IsInterface))
            {
                _types[item.Name] = item;
                _types[item.Namespace + "." + item.Name] = item;
                _types[item.FullName] = item;
            }
        }
        public static Dictionary<string, string> GetQueryStrings(HttpRequestMessage request)
        {
            return request.GetQueryNameValuePairs()
                          .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
        }

        [Route("types")]
        public IEnumerable<string> GetTypes()
        {
            return _types.Values.Select(a => a.Name);
        }

        [Route("send/{messageType}")]
        public void SendSync(HttpRequestMessage request)
        {
            var command = PrepareCommand(request);
            _bus.Send(command);
        }

        private ICommand PrepareCommand(HttpRequestMessage request)
        {
            var messageType = (string)this.ControllerContext.RouteData.Values["messageType"];
            var type = _types[messageType];

            var content = request.Content;
            string jsonContent = content.ReadAsStringAsync().Result;

            var command = (ICommand)JsonConvert.DeserializeObject(jsonContent, type);
            return command;
        }
    }
}
