using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crud;
using Cruise;
using Cruise.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace Real.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectionsController : ControllerBase
    {
        private IRepository<CruiseProjectionEntity> _cruises;
        private IRepository<RoomsForTripsEntity> _rooms;

        public ProjectionsController(
            IRepository<CruiseProjectionEntity> cruises,
            IRepository<RoomsForTripsEntity> rooms)
        {
            _cruises = cruises;
            _rooms = rooms;
        }

        [HttpGet("cruises")]
        public List<CruiseProjectionEntity> ListCruises()
        {
            return _cruises.GetAll().ToList();
        }

        [HttpGet("rooms")]
        public List<RoomsForTripsEntity> ListRooms()
        {
            return _rooms.GetAll().ToList();
        }
    }
}