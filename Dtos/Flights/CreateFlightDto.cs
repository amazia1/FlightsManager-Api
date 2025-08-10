namespace API.Dtos.Flights
{
    public class CreateFlightDto
    {
        public string Destination { get; set; } = string.Empty;

        public DateTime DepartureTime { get; set; }

        public string Gate { get; set; } = string.Empty;

    }
}