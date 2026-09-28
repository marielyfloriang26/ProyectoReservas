namespace Domain.Entities;
using System.ComponentModel.DataAnnotations; // <--- Esta es la que te falta

public class Property
{
    public int Id {get; set;}
    public string Titulo {get; set;} = string.Empty;
    public string? Descripcion {get; set;} // nullable. el ? dice que sí se permite que sea nulo.
    public string Ubicacion {get; set;} = string.Empty;
    
    // relacion con usuario (dueno)
    public int OwnerId {get; set;} // id USER solo numero, FK EN BD
    public User? Owner {get; set;} = null!;// objeto del usuario  ej: Owner.Nombre, Owner.Email
    // precio por noche
    public decimal PrecioNoche {get; set;} 
    public int Capacidad {get; set;}


    // disponibilidad y reglas 
    // Usar ICollection y virtual
    public virtual ICollection<Reservation> Reservas {get; set;} = new List<Reservation>();
    public virtual ICollection<Review> Resenas {get; set;} = new List<Review>();
    public virtual ICollection<BlockedDate> FechasBloqueadas {get; set;} = new List<BlockedDate>();
}