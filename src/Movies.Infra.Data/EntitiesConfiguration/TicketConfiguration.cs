using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies.Domain.Entities;

namespace Movies.Infra.Data.EntitiesConfiguration
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(entry => entry.Id);
            builder.HasOne(entry => entry.Showtime).WithMany(entry => entry.Tickets).HasForeignKey(entry => entry.ShowtimeId);
            builder.HasMany(entry => entry.BookedSeats).WithOne(entry => entry.Ticket).HasForeignKey(entry => entry.TicketId);
        }
    }
}
