using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly ApplicationDBContext _context;

    public PropertyRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task CrearPropiedad(Property propiedad)
    {
        await _context.Properties.AddAsync(propiedad);
        await _context.SaveChangesAsync();
    }

    public async Task<Property?> GetPorId(int id)
    {
        return await _context.Properties
            .Include(p => p.Owner)
            .Include(p => p.FechasBloqueadas)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Property>> GetTodas()
    {
        return await _context.Properties.ToListAsync();
    }

    public async Task ActualizarPropiedad(Property propiedad)
    {
        _context.Properties.Update(propiedad);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarPropiedad(int id)
    {
        var propiedad = await _context.Properties.FindAsync(id);
        if (propiedad != null)
        {
            _context.Properties.Remove(propiedad);
            await _context.SaveChangesAsync();
        }
    }
}