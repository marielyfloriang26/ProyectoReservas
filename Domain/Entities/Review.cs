using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class Review
{
    public int Id {get; set;}
    
    [Range(1, 5)]
    public int Calificacion {get; set;} // 1 al 5
    public string? Comentario {get; set;}
    public DateTime FechaCreado {get; set;} = DateTime.UtcNow; // 
    
    // relacion con property 
    public int PropertyId {get; set;} // FK de la Propiedad
    public Property? property {get; set;} = null!; // objeto de propiedad

    // relacion con User
    public int UserId {get; set;}
    public User user {get; set;} = null!;

    // relacion con reservation 
    public int ReservationId {get; set;}
    public Reservation reservation {get; set;} = null!;
}