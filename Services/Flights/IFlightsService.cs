
namespace API.Services.Flights
{
    public interface IFlightsService
    {
        Task<ApiResponse<FlightDto>> CreateFlight(CreateFlightDto createFlightDto);

        Task<ApiResponse<IEnumerable<FlightDto>>> GetAllFlights();

        Task<ApiResponse<IEnumerable<FlightDto>>> SearchFlights(StatusEnum? status, string? destination);

        Task<ApiResponse<object>> DeleteFlight(int flightNumber);
    }
}