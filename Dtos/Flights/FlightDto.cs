
namespace API.Dtos.Flights
{
    public class FlightDto
    {
        public int FlightNumber { get; set; }

        public string Destination { get; set; } = string.Empty;

        public DateTime DepartureTime { get; set; }

        public string Gate { get; set; } = string.Empty;

        public StatusEnum Status { get; set; }
    }
}