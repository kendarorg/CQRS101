using System.Collections.Generic;
using System.Linq;
using Crud;
using Cruise;
using Microsoft.AspNetCore.Mvc;

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