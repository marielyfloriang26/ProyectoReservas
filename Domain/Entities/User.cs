using Domain.Enums;


namespace Domain.Entities;
public class User
{
    public int Id {get; set;}
    // string.empty 
    public string Nombre {get; set;} = string.Empty; //  = string.Empty; ---> Es obligatorio que tenga texto. No puede ser nulo.
    public string Email {get; set;} = string.Empty;

    // Hash por seguridad
    public string PasswordHash {get; set;} = string.Empty;
    
    // reglas, propiedad de  confirmacion 
    public bool Confirmada {get; set;} = false; // aut empieza en false 
    
    // token y expiracion, el ? es porque deberia ser null, despues de usarlo se elimina o invalida
    public string? ConfirmacionToken {get; set;}
    public DateTime? ExpiracionToken {get; set;}

    
    // list en roles porque debe ser una coleccion, host, guest o ambos
    public List<UserRole> Roles {get; set;} = new List<UserRole>();

    // Propiedades de navegación usando ICollection (Mejor para EF Core)
    public virtual ICollection<Property> Propiedades {get; set;} = new List<Property>();
    public virtual ICollection<Reservation> Reservas {get; set;} = new List<Reservation>();
    public virtual ICollection<Review> Resenas {get; set;} = new List<Review>();
    public virtual ICollection<Notification> Notificaciones {get; set;} = new List<Notification>();

    // virtual: Permite que Entity Framework cargue los datos relacionados (como el nombre del dueño al ver una propiedad) de forma automática (Lazy Loading).
}