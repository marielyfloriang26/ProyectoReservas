
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;
public class ReservationConfiguration :     IEntityTypeConfiguration<Reservation>
    {
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        // nombre de la tabla
        builder.ToTable("Reservations");

        // llave primaria
        builder.HasKey(r => r.Id);

        // manejo de concurrencia
        builder.Property(r => r.RowVersion)
                .IsRowVersion();

           // Relación con la Propiedad
        builder.HasOne(r => r.property)
            .WithMany(p => p.Reservas)
            .HasForeignKey(r => r.PropertyId);

        // Relación con el Usuario (Guest)
        builder.HasOne(r => r.user)
            .WithMany(u => u.Reservas)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    }