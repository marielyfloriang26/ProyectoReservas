using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        // nombre de la tabla
        builder.ToTable("Notifications");

        builder.HasKey(n => n.Id);
        
        builder.HasOne(n => n.User)
            .WithMany(u => u.Notificaciones)
            .HasForeignKey(n => n.UserId);
    }
}