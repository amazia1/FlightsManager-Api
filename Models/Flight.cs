
namespace API.Models
{
    public class Flight
    {
        public int FlightNumber { get; set; }

        public string Destination { get; set; } = string.Empty;

        public DateTime DepartureTime { get; set; }

        public string Gate { get; set; } = string.Empty;

        public DateTime RowAddedDateTime { get; set; }
    }
}