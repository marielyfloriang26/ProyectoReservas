using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class Notification
{
    public int Id {get; set;}
    
    public string Titulo {get; set;} = string.Empty;
    public string? Mensaje {get; set;} = string.Empty; // [cite: 117]
    public NotificationStatus NotificationStatus {get; set;} = NotificationStatus.NoLeida; // tipo enum 
    public DateTime FechaCreacion {get; set;} = DateTime.UtcNow;

    // relacion con User
    public int UserId {get; set;} // [cite: 120, 123]
    public User User {get; set;} = null!;

}