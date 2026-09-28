using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Persistence;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class PropertyController : ControllerBase
{
    private readonly ApplicationDBContext _context;

    public PropertyController(ApplicationDBContext context)
    {
        _context = context;
    }

    // OBTENER TODAS LAS PROPIEDADES
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<PropertyDto>>> GetAll()
    {
       var properties = await _context.Properties
        .Include(p => p.Owner)
        .Select(p => new PropertyDto {
            Id = p.Id,
            Titulo = p.Titulo,
            Ubicacion = p.Ubicacion,
            PrecioNoche = p.PrecioNoche,
            NombreOwner = p.Owner!.Nombre
        }).ToListAsync();

    return Ok(properties);
        // return await _context.Properties.Include(p => p.Owner).ToListAsync();
    }

   // [Authorize(Roles = "Host")] // SOLO usuarios con el rol Host pueden entrar aquí
    // CREAR PROPIEDAD 
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PropertyDto propertyDto)
    {
        // Validar que el dueño (User) exista
        var ownerExists = await _context.Users.AnyAsync(u => u.Id == propertyDto.OwnerId);
        if (!ownerExists)
        {
            return BadRequest("El ID del dueño (OwnerId) no existe en el sistema.");
        }

        // Mapeo manual del DTO a la Entidad Property
        var nuevaPropiedad = new Property
        {
            Titulo = propertyDto.Titulo,
            Descripcion = propertyDto.Descripcion,
            Ubicacion = propertyDto.Ubicacion,
            OwnerId = propertyDto.OwnerId,
            PrecioNoche = propertyDto.PrecioNoche,
            Capacidad = propertyDto.Capacidad
        };

        _context.Properties.Add(nuevaPropiedad);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = nuevaPropiedad.Id }, nuevaPropiedad);
    }

    //[Authorize] // <--- Cualquier usuario autenticado (Host o Guest) puede ver esto
    // OBTENER POR ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Property>> GetById(int id)
    {
        var property = await _context.Properties
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (property == null)
        {
            return NotFound("Propiedad no encontrada.");
        }

        return property;
    }

    // ELIMINAR PROPIEDAD
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var property = await _context.Properties.FindAsync(id);
        if (property == null)
        {
            return NotFound();
        }

        _context.Properties.Remove(property);
        await _context.SaveChangesAsync();

        return Ok("Propiedad eliminada correctamente.");
    }
}
