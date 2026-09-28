using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.DTOs;
using Infrastructure.Persistence;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly ApplicationDBContext _context;

    public ReviewController(ApplicationDBContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReviewDto reviewDto)
    {
        // Validar que la reserva exista
        var reservation = await _context.Reservations.FindAsync(reviewDto.ReservationId);
        if (reservation == null) return BadRequest("La reserva no existe.");

        // Mapear DTO a Entidad
        var nuevaReview = new Review
        {
            Calificacion = reviewDto.Calificacion,
            Comentario = reviewDto.Comentario,
            ReservationId = reviewDto.ReservationId,
            // Extraemos automáticamente la propiedad y el usuario de la reserva
            PropertyId = reservation.PropertyId,
            UserId = reservation.UserId,
            FechaCreado = DateTime.Now
        };

        _context.Reviews.Add(nuevaReview);
        await _context.SaveChangesAsync();

        return Ok("¡Gracias por tu calificación!");
    }

    [HttpGet("property/{propertyId}")]
    public async Task<IActionResult> GetByProperty(int propertyId)
    {
        var reviews = await _context.Reviews
            .Where(r => r.PropertyId == propertyId)
            .Include(r => r.user)
            .ToListAsync();
        return Ok(reviews);
    }
}
