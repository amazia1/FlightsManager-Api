using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Flight, FlightDto>();
            CreateMap<CreateFlightDto, Flight>();
        }
    }
}