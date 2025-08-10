using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightsService _flightsService;

        public FlightsController(IFlightsService flightsService)
        {
            _flightsService = flightsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightDto>>> GetAll()
        {
            var response = await _flightsService.GetAllFlights();

            if (response.Success is false)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<FlightDto>>> Search(StatusEnum? status = null, string? destination = null)
        {
            var response = await _flightsService.SearchFlights(status, destination);
            if (response.Success is false)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<FlightDto>> CreateFlight(CreateFlightDto createFlightDto)
        {
            var response = await _flightsService.CreateFlight(createFlightDto);
            if (response.Success is false)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteFlight([Required]int flightNumber)
        {
            var response = await _flightsService.DeleteFlight(flightNumber);
            if (response.Success is false)
                return StatusCode(StatusCodes.Status500InternalServerError, response);
                
            return NoContent();
        }
    }
}