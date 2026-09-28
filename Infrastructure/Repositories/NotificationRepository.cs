using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly ApplicationDBContext _context;

    // Constructor: Recibimos el contexto de la base de datos
    public NotificationRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    // Guarda una nueva notificación (ej: "Tu reserva fue confirmada")
    public async Task GuardarNotificacion(Notification notificacion)
    {
        await _context.Notifications.AddAsync(notificacion);
        await _context.SaveChangesAsync(); // Guardamos cambios en la BD
    }

    // Obtiene todo el historial de notificaciones de un usuario
    public async Task<List<Notification>> GetPorUsuario(int userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.FechaCreacion) // las mas recientes
            .ToListAsync();
    }

    // Filtra solo las notificaciones que el usuario aún no ha visto
    public async Task<List<Notification>> GetNoLeidas(int userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId && n.NotificationStatus == Domain.Enums.NotificationStatus.NoLeida)
            .ToListAsync();
    }

    // Cambia el estado de una notificación a "Leída"
    public async Task MarcarComoLeida(int notificacionId)
    {
        var noti = await _context.Notifications.FindAsync(notificacionId);
        if (noti != null)
        {
            noti.NotificationStatus = Domain.Enums.NotificationStatus.Leida;
            await _context.SaveChangesAsync();
        }
    }


}