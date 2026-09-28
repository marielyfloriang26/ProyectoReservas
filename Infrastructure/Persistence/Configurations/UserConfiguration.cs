using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // nombre de la tabla
        builder.ToTable("User");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(150);

        // email no puede repetirse en la base de datos
        builder.HasIndex(u => u.Email).IsUnique();

        // config de los roles (se guarda como string o se puede mapear)
        builder.Property(u => u.Roles)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(r => Enum.Parse<Domain.Enums.UserRole>(r)).ToList());
    }
}