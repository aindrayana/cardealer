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
            await context.SaveChangesAsync();   // SaveChangesAsync has caused a loop on domain model

            // add a new mapping on MappingProfile that do mapping reversal
            // after save the changes we going to map the vehicle object back to vehicleResource
            // this will prevent Self referencing loop errors
            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(result);
        }
    }
}