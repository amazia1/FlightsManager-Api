
using System.Collections;
using API.Data;

namespace API.Repositories.Flights
{
    public class FlightsRepository : IFlightsRepository
    {
        private readonly DataContext _flightsContext;

        public FlightsRepository(DataContext flightsContext)
        {
            _flightsContext = flightsContext;
        }

        public async Task<Flight> CreateFlight(Flight flight)
        {
            await _flightsContext.Flights.AddAsync(flight);
            await _flightsContext.SaveChangesAsync();
            return flight;
        }

        public async Task DeleteFlight(int deletedFlight)
        {
            var flight = await _flightsContext.Flights.FindAsync(deletedFlight);
            if (flight is not null)
            {
                _flightsContext.Flights.Remove(flight);
            }
        }

        public IQueryable<Flight> GetAllFlights()
        {
            return _flightsContext.Flights.AsNoTracking();
        }

        public async Task SaveChanges()
        {
            await _flightsContext.SaveChangesAsync();
        }
    }
}