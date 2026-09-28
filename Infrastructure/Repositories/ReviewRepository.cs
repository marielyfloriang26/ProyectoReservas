using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ApplicationDBContext _context;

    public ReviewRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    // Registra la calificación y el comentario en la tabla Reviews
    public async Task CrearResena(Review review)
    {
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
    }

    // Obtiene todas las reseñas de una propiedad específica para calcular su promedio
    public async Task<List<Review>> ObtenerPorPropiedad(int propertyId)
    {
        return await _context.Reviews
            .Where(r => r.PropertyId == propertyId)
            .Include(r => r.user) // Incluimos quién escribió la reseña
            .ToListAsync();
    }

    // Busca si ya existe una reseña vinculada a una reserva específica
    public async Task<Review?> ObtenerPorReservaId(int reservationId)
    {
        return await _context.Reviews
            .FirstOrDefaultAsync(r => r.ReservationId == reservationId);
    }
}