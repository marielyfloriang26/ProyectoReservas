using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class BlockedDateConfiguration : IEntityTypeConfiguration<BlockedDate>
{
    public void Configure(EntityTypeBuilder<BlockedDate> builder)
    {
        builder.ToTable("BlockedDates"); // Nombre de la tabla

        builder.HasKey(b => b.Id);

        // Relación con Property
        builder.HasOne(b => b.Property)
            .WithMany(p => p.FechasBloqueadas)
            .HasForeignKey(b => b.PropertyId)
            .OnDelete(DeleteBehavior.Cascade); // Si se borra la propiedad, se borran sus bloqueos
    }
}