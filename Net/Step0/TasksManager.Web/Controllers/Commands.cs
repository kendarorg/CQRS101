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
        private IBus _bus;

        public CommandsController(IBus bus)
        {
            _bus = bus;
        }
        
        

        [HttpGet]
        [Route("types")]
        public IEnumerable<string> GetTypes()
        {
            return _bus.GetTypes();
        }

        [HttpPost]
        [Route("send/{messageType}")]
        public void SendMessage(HttpRequestMessage request)
        {
            var command = PrepareCommand(request);
            _bus.Send(command);
        }

        public static Dictionary<string, string> GetQueryStrings(HttpRequestMessage request)
        {
            return request.GetQueryNameValuePairs()
                          .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
        }

        private ICommand PrepareCommand(HttpRequestMessage request)
        {
            var messageType = (string)this.ControllerContext.RouteData.Values["messageType"];
            var type = _bus.GetType(messageType);

            var content = request.Content;
            string jsonContent = content.ReadAsStringAsync().Result;

            var command = (ICommand)JsonConvert.DeserializeObject(jsonContent, type);
            return command;
        }
    }
}
