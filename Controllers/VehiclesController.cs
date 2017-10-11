using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using vega.Controllers.Resources;
using vega.Models;
using vega.Persistence;

namespace vega.Controllers
{
    [Route("/api/vehicles")]
    public class VehiclesController : Controller
    {
        // to use automapper we need to inject IMapper interface to constructor
        private readonly IMapper mapper;
        // inject dbContext to VehicleController
        private readonly VegaDbContext context;
        public VehiclesController(IMapper mapper, VegaDbContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] VehicleResource vehicleResource)
        {
            // map api resource to domain object
            var vehicle = mapper.Map<VehicleResource, Vehicle>(vehicleResource);
            // add to context and save the changes
            context.Vehicles.Add(vehicle);
            await context.SaveChangesAsync();
            return Ok(vehicle);
        }
    }
}