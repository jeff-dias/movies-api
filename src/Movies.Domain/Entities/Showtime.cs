using Movies.Domain.Validation;

namespace Movies.Domain.Entities
{
    public sealed class Showtime : Entity
    {
        public DateTime SessionDate { get; private set; }
        public string ExternalMovieId { get; private set; }
        public Guid AuditoriumId { get; private set; }
        public Auditorium? Auditorium { get; private set; }
        public ICollection<Ticket>? Tickets { get; private set; }

        public Showtime(DateTime sessionDate, Guid auditoriumId, string externalMovieId) : base()
        {
            Validate(sessionDate, auditoriumId, externalMovieId);
            SessionDate = sessionDate;
            AuditoriumId = auditoriumId;
            ExternalMovieId = externalMovieId;
        }

        private void Validate(DateTime sessionDate, Guid auditoriumId, string externalMovieId)
        {
            DomainExceptionValidation.When(sessionDate <= DateTime.Now, "Date is old, you can create an event only for future");
            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(externalMovieId), "External movie id is required");
            DomainExceptionValidation.When(auditoriumId == Guid.Empty, "Auditorium is required");
        }
    }
}
