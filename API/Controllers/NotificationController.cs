using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.DTOs;
using Domain.Enums; // Para que reconozca NotificationStatus
using Infrastructure.Persistence;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly ApplicationDBContext _context;

    public NotificationController(ApplicationDBContext context)
    {
        _context = context;
    }

    // CREAR NOTIFICACIÓN
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NotificationDto notiDto)
    {
        var nuevaNoti = new Notification
        {
            Mensaje = notiDto.Mensaje,
            // Usamos la propiedad que sí tienes en tu Entidad/DTO
            NotificationStatus = notiDto.NotificationStatus, 
            FechaCreacion = notiDto.FechaCreacion == default ? DateTime.Now : notiDto.FechaCreacion,
            UserId = notiDto.UserId
        };

        _context.Notifications.Add(nuevaNoti);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Notificación enviada", id = nuevaNoti.Id });
    }

    // OBTENER NOTIFICACIONES DE UN USUARIO
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var notis = await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.FechaCreacion)
            .ToListAsync();
            
        return Ok(notis);
    }

    // ACTUALIZAR ESTADO (En lugar de 'leida', usamos tu Enum)
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] NotificationStatus nuevoEstado)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification == null) return NotFound();

        notification.NotificationStatus = nuevoEstado;
        await _context.SaveChangesAsync();
        
        return Ok("Estado de notificación actualizado.");
    }
}
