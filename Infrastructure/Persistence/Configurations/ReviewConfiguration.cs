using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        // nombre de la tabla
        builder.ToTable("Review");

        builder.HasKey(re => re.Id);
        
        // Relación con Reserva (Una reseña por reserva)
        builder.HasOne(re => re.reservation)
            .WithMany()
            .HasForeignKey(re => re.ReservationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}