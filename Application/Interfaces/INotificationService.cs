using Domain.Entities;

using Application.DTOs;

namespace Application.Interfaces;

public interface INotificationService
{
    // El PDF dice: "Las notificaciones deben generarse automáticamente"
    // Este método lo llamarán otros servicios (como el de Reservas)
    Task CrearNotificacion(int userId, string titulo, string mensaje);

    // El PDF dice: "Un usuario solo puede consultar sus propias notificaciones"
    Task<List<NotificationDto>> ObtenerNotificacionesUsuario(int userId);

    // El PDF dice: "El sistema debe permitir filtrar no leídas"
    Task<List<NotificationDto>> ObtenerNoLeidas(int userId);

    // El PDF dice: "El sistema debe permitir marcar como leídas"
    Task MarcarComoLeida(int notificationId, int userId);
}