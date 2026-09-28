namespace Application.DTOs;

public class PropertyDto
{
    public int Id { get; set; }
    public string Titulo {get; set;}
    public string? Descripcion {get; set;} // nullable
    public string Ubicacion {get; set;}
    public string? NombreOwner { get; set; }
    
    // owner id, id del dueno
    public int OwnerId {get; set;}
    // precio por noche
    public decimal PrecioNoche {get; set;} 
    public int Capacidad {get; set;}

}