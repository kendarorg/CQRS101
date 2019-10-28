using System.Threading.Tasks;
using Cruise.Commands;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace Real.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly IMessageSession _messageSession;

        public CommandsController(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        [HttpPost("create/cruise")]
        public async Task<string> CreateCruise(CreateCruise command)
        {
            await _messageSession.Send(command)
                .ConfigureAwait(false);
            return "Message sent to endpoint";
        }

        [HttpPost("create/room")]
        public async Task<string> CreateRoom(CreateRoom command)
        {
            await _messageSession.Send(command)
                .ConfigureAwait(false);
            return "Message sent to endpoint";
        }
    }
}