using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        // nombre de la tabla
        builder.ToTable("Property");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Titulo).IsRequired().HasMaxLength(200);
        builder.Property(p => p.PrecioNoche).HasColumnType("decimal(18,2)");

        // Relación: Un dueño (User) tiene muchas propiedades
        builder.HasOne(p => p.Owner)
            .WithMany(u => u.Propiedades)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}