using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.DTOs;
using Infrastructure.Persistence;
 

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ApplicationDBContext _context;

    public UserController(ApplicationDBContext context)
    {
        _context = context;
    }

    // 1. OBTENER TODOS LOS USUARIOS
    // Útil para verificar en Swagger que se están guardando
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    // 2. OBTENER UN USUARIO POR su ID
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound("Usuario no encontrado.");
        }

        return user;
    }

    // 3. ACTUALIZAR PERFIL (PUT)
    // Aquí podría usar un UserUpdateDto si solo se cambiara el nombre
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RegisterUserDto updateDto)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        user.Nombre = updateDto.Nombre;
        user.Email = updateDto.Email;
        // Solo actualizamos el password si viene algo en el DTO
        if (!string.IsNullOrEmpty(updateDto.Password))
        {
            user.PasswordHash = updateDto.Password; 
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // 4. ELIMINAR USUARIO
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok("Usuario eliminado correctamente.");
    }
/*
    // 5. CONVERTIR USUARIO EN ANFITRIÓN
    [HttpPost("BecomeHost/{id}")]
    public async Task<IActionResult> BecomeHost(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound("Usuario no encontrado.");
        }

        // Suponiendo que el campo de la tabla se llama Role o Rol
        // Si tienes problemas de compilación, ajusta el nombre a tu propiedad real
        user.Role = "Host"; 

        await _context.SaveChangesAsync();
        return Ok("¡Felicidades! Ahora tienes el perfil de Anfitrión.");
    } */
}

