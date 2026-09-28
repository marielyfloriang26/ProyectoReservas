using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BlockedDateRepository : IBlockedDateRepository
{
    private readonly ApplicationDBContext _context;

    public BlockedDateRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    // El Host selecciona un rango de fechas para bloquear su propiedad
    public async Task CrearFechaBloqueada(BlockedDate fecha)
    {
        await _context.BlockedDates.AddAsync(fecha);
        await _context.SaveChangesAsync();
    }

    // Busca todos los bloqueos actuales de una propiedad
    public async Task<List<BlockedDate>> ObtenerPorPropiedad(int propertyId)
    {
        return await _context.BlockedDates
            .Where(b => b.PropertyId == propertyId)
            .ToListAsync();
    }

    // Si el Host cambia de opinión, puede eliminar el bloqueo por su ID
    public async Task EliminarFechaBloqueada(int id)
    {
        var fecha = await _context.BlockedDates.FindAsync(id);
        if (fecha != null)
        {
            _context.BlockedDates.Remove(fecha);
            await _context.SaveChangesAsync();
        }
    }
}