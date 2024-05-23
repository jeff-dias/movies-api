using AutoMapper;
using Movies.Application.DTOs;
using Movies.Application.Mediators.Showtimes.Commands;
using Movies.Application.Mediators.Tickets.Commands;

namespace Movies.Application.Mappings
{
    public class DtoToCommandMappingProfile : Profile
    {
        public DtoToCommandMappingProfile()
        {
            CreateMap<ShowtimeDto, ShowtimeCommand>();
            CreateMap<ShowtimeDto, ShowtimeCreateCommand>();

            CreateMap<TicketDto, TicketCreateCommand>();
        }
    }
}
