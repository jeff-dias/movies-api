using AutoMapper;
using Movies.Application.DTOs;
using Movies.Domain.Entities;

namespace Movies.Application.Mappings
{
    public class DomainToDtoMappingProfile : Profile
    {
        public DomainToDtoMappingProfile()
        {
            CreateMap<Auditorium, AuditoriumDto>().ReverseMap();
            CreateMap<BookedSeat, BookedSeatDto>().ReverseMap();
            CreateMap<Seat, SeatDto>().ReverseMap();
            CreateMap<Showtime, ShowtimeDto>().ReverseMap();
            CreateMap<Ticket, TicketDto>().ReverseMap();
        }
    }
}
