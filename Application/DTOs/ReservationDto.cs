using Domain.Enums;

namespace Application.DTOs;

public class ReservationDto
{
    public DateTime FechaInicio {get; set;}
    public DateTime FechaFin {get; set;}
    
    // relacion con property 
    public int PropertyId {get; set;} // FK de la Propiedad
    
    // relacion con User
    public int UserId {get; set;}
}