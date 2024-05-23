namespace Movies.Domain.Entities
{
    public sealed class Auditorium : Entity
    {
        public ICollection<Showtime> Showtimes { get; private set; }
        public ICollection<Seat> Seats { get; private set; }

        public Auditorium() : base()
        {
            Showtimes = new List<Showtime>();
            Seats = new List<Seat>();
        }

        public void Update()
        {
            UpdatedAt = DateTime.Now;
        }

        public void Delete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.Now;
        }
    }
}
