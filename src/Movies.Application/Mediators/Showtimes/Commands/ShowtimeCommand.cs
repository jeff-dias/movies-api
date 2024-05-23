using MediatR;
using Movies.Domain.Entities;

namespace Movies.Application.Mediators.Showtimes.Commands
{
    public class ShowtimeCommand : IRequest<Showtime>
    {
        public DateTime SessionDate { get; set; }
        public string ExternalMovieId { get; set; }
        public Guid AuditoriumId { get; set; }
        public Auditorium? Auditorium { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
    }
}
