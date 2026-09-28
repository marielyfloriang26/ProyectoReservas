using System.ComponentModel.DataAnnotations;


namespace Application.DTOs;

public class ReviewDto
{    
    [Range(1, 5)]
    public int Calificacion {get; set;} // 1 al 5
    public string? Comentario {get; set;}
    
    // relacion con reservation 
    public int ReservationId {get; set;}
   
}