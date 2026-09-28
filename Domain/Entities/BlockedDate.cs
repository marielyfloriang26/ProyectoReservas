using Domain.Enums;

namespace Domain.Entities;

public class BlockedDate
{
    public int Id {get; set;}
   // public DateTime FechaBloqueada {get; set;}
   public DateTime FechaInicio {get; set;} // Cambiar por rango
   public DateTime FechaFin {get; set;}
   
    // relacion con property 
    public int PropertyId {get; set;} // FK de la Propiedad
    public Property? Property {get; set;} = null!; // objeto de propiedad

    // null!: Es una instrucción para decirle al compilador: "Sé que esto parece nulo ahora, pero la base de datos se encargará de llenarlo, así que no me des advertencias".
}