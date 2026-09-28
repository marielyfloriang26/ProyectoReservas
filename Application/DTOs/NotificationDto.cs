
using Domain.Enums;

namespace Application.DTOs;

public class NotificationDto
{
    public string? Mensaje {get; set;}
    public NotificationStatus NotificationStatus {get; set;} // tipo enum
    public DateTime FechaCreacion {get; set;}

    // relacion con User
    public int UserId {get; set;}

}