using Microsoft.EntityFrameworkCore;
using Domain.Entities;


namespace Infrastructure.Persistence;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) {}

    // tablas de las bases de datos 
    public DbSet<User> Users {get; set;}
    public DbSet<Property> Properties {get; set;}
    public DbSet<Reservation> Reservations {get; set;}
    public DbSet<Notification> Notifications {get; set;}
    public DbSet<Review> Reviews {get; set;}
    public DbSet<BlockedDate> BlockedDates {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Esto busca automáticamente todas las clases que hereden de IEntityTypeConfiguration 
        // en este mismo proyecto y las aplica
        // // Esto le dice a EF: "Ve a buscar las configuraciones que hicimos en la otra carpeta"
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);

    }
}