using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Lib.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using SimplestPossibleThing.Lib.Commands;

namespace SimplestPossibleThing.Controllers
{
    [Route("api/inventory/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private IBus _bus;

        public CommandsController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        [Route("")]

        public OkResult Create(CreateInventoryItem createInventoryItem)
        {
            _bus.Send(createInventoryItem);
            return new OkResult();
        }

        [HttpPut]
        [Route("rename")]

        public OkResult Rename(RenameInventoryItem renameInventoryItem)
        {
            _bus.Send(renameInventoryItem);
            return new OkResult();
        }

        [HttpPut]
        [Route("checkin")]

        public OkResult CheckIn(CheckInItemsToInventory checkInItemsToInventory)
        {
            _bus.Send(checkInItemsToInventory);
            return new OkResult();
        }

        [HttpPut]
        [Route("deactivate")]

        public OkResult Rename(DeactivateInventoryItem deactivateInventoryItem)
        {
            _bus.Send(deactivateInventoryItem);
            return new OkResult();
        }

        [HttpPut]
        [Route("remove")]

        public OkResult Remove(RemoveItemsFromInventory removeItemsFromInventory)
        {
            _bus.Send(removeItemsFromInventory);
            return new OkResult();
        }
    }
}
