using Microsoft.AspNetCore.Mvc;
using SimplestPossibleThing.Lib.Projection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplestPossibleThing.Controllers
{
    [Route("api/inventory")]
    [ApiController]
    public class ProjectionsController:ControllerBase
    {
        private readonly IInventoryItemListRepository _inventoryItemListRepository;
        private readonly IInventoryItemDetailsRepository _inventoryItemDetailsRepository;

        public ProjectionsController(
            IInventoryItemListRepository inventoryItemListRepository,
            IInventoryItemDetailsRepository inventoryItemDetailsRepository)
        {
            _inventoryItemListRepository = inventoryItemListRepository;
            _inventoryItemDetailsRepository = inventoryItemDetailsRepository;
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<InventoryItemListDto> GetAll()
        {
            return _inventoryItemListRepository.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public InventoryItemDetailsDto GetById(Guid id)
        {
            return _inventoryItemDetailsRepository.GetById(id);
        }
    }
}
