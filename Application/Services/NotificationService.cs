using System;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;


namespace Application.Services;

public class NotificationService
{
    // variables privadas, dependencias
    private readonly INotificationRepository _notificationRepository;
    private readonly IEmailService _emailService;

    // constructor, inyeccion de dependencias
    public NotificationService(INotificationRepository notificationRepository, IEmailService emailService)
    {
        this._notificationRepository = _notificationRepository; // asigna parametro a varible

        this._emailService = _emailService;
        
    }

    public async Task CrearNotificacion(Notification notificacion) 
    {
        //Asignar fecha automática si no viene
        if(notificacion.FechaCreacion == default)
        {
            notificacion.FechaCreacion = DateTime.Now;
        }
        // Guardar en BD
        await _notificationRepository.GuardarNotificacion(notificacion);

        // Opcional: Intentar enviar email (si el servicio está listo)
        await _emailService.EnviarCorreoConfirmacionAsync(notificacion.UserId.ToString(), "Nueva Notificación", notificacion.Mensaje);
    }
    public async Task <List<Notification>> ObtenerNotificacionesUsuario(int userId) 
    {
      // llama al repo para traer el historial de usuario 
      return await _notificationRepository.GetPorUsuario(userId);
    }

    public async Task <List<Notification>> ObtenerNoLeidas(int userId) 
    {
       return await _notificationRepository.GetNoLeidas(userId);
    }
    public async Task MarcarComoLeida(int notificacionId) 
    {
        // Buscamos las notificaciones del usuario o la específica.
     var notificacion = await _notificationRepository.GetPorUsuario(notificacionId);

     // Buscamos la notificación específica dentro de la lista
    var notificacionUnica = notificacion.FirstOrDefault(n => n.Id == notificacionId);

     if(notificacionUnica != null)
        {
            notificacionUnica.NotificationStatus = NotificationStatus.Leida;

            await _notificationRepository.MarcarComoLeida(notificacionId);
        }
        else
        {
            throw new Exception("Notificación no encontrada.");
        }
    }
}