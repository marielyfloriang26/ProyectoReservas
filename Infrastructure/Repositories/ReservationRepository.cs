using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ApplicationDBContext _context;

    public ReservationRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task CrearReserva(Reservation reserva)
    {
        await _context.Reservations.AddAsync(reserva);
        await _context.SaveChangesAsync();
    }
    public async Task<Reservation?> GetPorId(int id)
{
    return await _context.Reservations
        .Include(r => r.property)// Carga la casa
            .ThenInclude(p => p.Owner)// Carga al dueño de la casa
        .Include(r => r.user)// Carga al cliente (Guest)
        .FirstOrDefaultAsync(r => r.Id == id);
}
    public async Task<List<Reservation>> GetPorPropiedad(int propertyId)
    {
        return await _context.Reservations
            .Where(r => r.PropertyId == propertyId)
            .ToListAsync();
    }
    public async Task<List<Reservation>> GetPorUsuario(int userId)
    {
        return await _context.Reservations
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    public async Task ActualizarReserva(Reservation reserva)
    {
        _context.Reservations.Update(reserva);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Reservation>> GetTodas() 
    {
    return await _context.Reservations.ToListAsync();
    }
    
}