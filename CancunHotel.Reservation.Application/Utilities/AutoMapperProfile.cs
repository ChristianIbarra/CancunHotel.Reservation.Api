using AutoMapper;

namespace CancunHotel.Reservation.Application.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Domain.Reservation, DTOs.Reservation>();
        }
    }
}
