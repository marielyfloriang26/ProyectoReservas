using Domain.Enums;

namespace Domain.Entities;

public class Reservation
{
    public int Id {get; set;}
    public DateTime FechaInicio {get; set;}
    public DateTime FechaFin {get; set;}
    public decimal PrecioTotal { get; set; }
    public ReservationStatus EstadoReserva {get; set;} = ReservationStatus.Confirmada;
    public DateTime FechaCreacion {get; set;}
    
     // obligatorio: manejo de concurrencias
    // para concurrencia, relacion con rowversion
    public byte[] RowVersion {get; set;} = null!;
    // relacion con property 
    public int PropertyId {get; set;} // FK de la Propiedad
    public Property property {get; set;} = null!; // objeto de propiedad

    // relacion con User
    public int UserId {get; set;} // el guest que reserva [cite: 12s]
    public User user {get; set;} = null!;

}