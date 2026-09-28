
using Domain.Entities;

namespace Domain.Interfaces;
public interface INotificationRepository
{
    Task GuardarNotificacion(Notification notificacion);
    Task<List<Notification>> GetPorUsuario(int userId);
    Task<List<Notification>> GetNoLeidas(int userId);
    Task MarcarComoLeida(int notificacionId);
}