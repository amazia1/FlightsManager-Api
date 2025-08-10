
namespace API.Repositories.Flights
{
    public interface IFlightsRepository
    {
        IQueryable<Flight> GetAllFlights();

        Task<Flight> CreateFlight(Flight flight);

        Task DeleteFlight(int FlightNumber);

        Task SaveChanges();
    }
}