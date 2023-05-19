using DE1805Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DE1805Api.Controllers
{
    [ApiController]
    public class StationsController : ControllerBase
    {

        private readonly ILogger<StationsController> _logger;

        public StationsController(ILogger<StationsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("stations")]
        public async Task<IActionResult> GetStations(
            [FromQuery(Name = "fuel")] string fuel,
            [FromServices] ApplicationContext context
        ) {
            if (fuel == "DT") fuel = FuelTypes.DT;
            if (!FuelTypes.List.Contains(fuel)) 
                return Problem(title: $"Параметр fuel может приниать значения: {String.Join(", ", FuelTypes.List)}.", statusCode: 400);

            var stations = await context.CarFillingStations.Where(s => s.Data.Any(d => d.Name == fuel)).ToListAsync();

            return Ok(stations.Select(s => new { s.Id, s.Address }));
        }

        [HttpGet("getStationInfo")]
        public async Task<IActionResult> GetStationInfo(
            [FromQuery(Name = "id")] int id,
            [FromServices] ApplicationContext context
        ) {
            var station = await context.CarFillingStations.Where(s => s.Id == id)
                    .Include(s => s.Data).FirstOrDefaultAsync();

            if (station == null) return NotFound();
            return Ok(station);
        }
        
        [HttpPost("setStation")]
        public async Task<IActionResult> SetStation(
            [FromServices] ApplicationContext context,
            [FromBody] CarFillingStationVM newStationModel
        ) {
            var newStation = newStationModel.ToOrigin();
            var existingStation = await context.CarFillingStations.Where(s => s.Id == newStation.Id)
                    .Include(s => s.Data).FirstOrDefaultAsync();

            if (existingStation == null)
            {
                context.Add(newStation);
                await context.SaveChangesAsync();
                return NoContent();
            }

            var existingStationFuel = existingStation.Data;
            var newStationFuel = newStation.Data;

            existingStation!.Address = newStation.Address;

            foreach (var newFuel in newStationFuel)
            {
                var existingFuel = existingStationFuel.FirstOrDefault(f => f.Name == newFuel.Name);

                if (existingFuel == null)
                    existingStationFuel.Add(newFuel);
                else
                    (existingFuel.Price, existingFuel.AmountOfFuel) = (newFuel.Price, newFuel.AmountOfFuel);
            }

            await context.SaveChangesAsync();

            return Ok();
        }

    }
}