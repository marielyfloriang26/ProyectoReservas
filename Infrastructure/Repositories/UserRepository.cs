using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDBContext _context;

    public UserRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task GuardarUsuario(User usuario)
    {
        await _context.Users.AddAsync(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetPorId(int id)
    {
        return await _context.Users.FindAsync(id);
    }
    public async Task<List<User>> ObtenerTodos()
    {
        // Usamos ToListAsync() para traer todos los registros de la tabla Users
        return await _context.Users.ToListAsync();
    }
    public async Task<User?> GetPorEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    public async Task ActualizarUsuario(User usuario)
    {
        _context.Users.Update(usuario);
        await _context.SaveChangesAsync();
    }
    
}