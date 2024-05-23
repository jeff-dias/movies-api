using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies.Domain.Entities;

namespace Movies.Infra.Data.EntitiesConfiguration
{
    public class BookedSeatConfiguration : IEntityTypeConfiguration<BookedSeat>
    {
        public void Configure(EntityTypeBuilder<BookedSeat> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(entry => entry.Ticket).WithMany(entry => entry.BookedSeats).HasForeignKey(entry => entry.TicketId);
            builder.HasOne(entry => entry.Seat).WithMany(entry => entry.BookedSeats).HasForeignKey(entry => entry.SeatId);
        }
    }
}
