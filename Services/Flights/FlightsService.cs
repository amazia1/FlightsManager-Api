namespace API.Services.Flights
{
    public class FlightsService : IFlightsService
    {
        private readonly IMapper _mapper;
        private readonly IFlightsRepository _flightsRepository;
        private readonly INotificationService _notifier;

        public FlightsService(IMapper mapper, IFlightsRepository flightsRepository, INotificationService notifier)
        {
            _mapper = mapper;
            _flightsRepository = flightsRepository;
            _notifier = notifier;
        }

        public async Task<ApiResponse<FlightDto>> CreateFlight(CreateFlightDto createFlightDto)
        {
            ApiResponse<FlightDto> response = new();

            try
            {
                Flight newFlight = _mapper.Map<Flight>(createFlightDto);
                newFlight.RowAddedDateTime = DateTime.Now;
                Flight createdFlight = await _flightsRepository.CreateFlight(newFlight);
                FlightDto dto = _mapper.Map<FlightDto>(createdFlight);
                dto.Status = SetFlightStatus(dto.DepartureTime);
                response.Data = dto;

                await _notifier.NotifyAllAsync("FlightCreated", dto);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ApiResponse<object>> DeleteFlight(int flightNumber)
        {
            ApiResponse<object> response = new();

            try
            {
                await _flightsRepository.DeleteFlight(flightNumber);
                await _flightsRepository.SaveChanges();
                response.Data = null;

                await _notifier.NotifyAllAsync("FlightDeleted", flightNumber);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ApiResponse<IEnumerable<FlightDto>>> GetAllFlights()
        {
            ApiResponse<IEnumerable<FlightDto>> response = new();

            try
            {
                var flightsQuery = _flightsRepository.GetAllFlights();
                var flights = await flightsQuery.ToListAsync();

                response.Data = flights.Select(f =>
                {
                    var flightDto = _mapper.Map<FlightDto>(f);
                    flightDto.Status = SetFlightStatus(flightDto.DepartureTime);
                    return flightDto;
                });
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ApiResponse<IEnumerable<FlightDto>>> SearchFlights(StatusEnum? status, string? destination)
        {
            ApiResponse<IEnumerable<FlightDto>> response = new();

            try
            {
                var flightsQuery = _flightsRepository.GetAllFlights();

                if (destination is not null)
                    flightsQuery = flightsQuery.Where(f => f.Destination.ToLower().Contains(destination.ToLower()));

                var flights = await flightsQuery.ToListAsync();

                var flightsDtos = flights.Select(fl =>
                {
                    var flightDto = _mapper.Map<FlightDto>(fl);
                    flightDto.Status = SetFlightStatus(flightDto.DepartureTime);
                    return flightDto;
                });

                if (status.HasValue)
                {
                    flightsDtos = flightsDtos.Where(f => status == SetFlightStatus(f.DepartureTime));
                }

                response.Data = flightsDtos;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        private StatusEnum SetFlightStatus(DateTime departureTime)
        {
            var now = DateTime.Now;

            return now switch
            {
                var n when n > departureTime.AddMinutes(-30) &&
                n < departureTime => StatusEnum.Boarding,

                var n when n > departureTime &&
                n < departureTime.AddMinutes(60) => StatusEnum.Departed,

                var n when n > departureTime.AddMinutes(60) => StatusEnum.Landed,

                _ => StatusEnum.Scheduled
            };
        }
    }
}