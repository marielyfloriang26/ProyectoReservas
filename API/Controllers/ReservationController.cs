using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.DTOs;
using Domain.Enums;
using Infrastructure.Persistence;
using Application.Services;

[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
   /* private readonly ApplicationDBContext _context;

    public ReservationController(ApplicationDBContext context)
    {
        _context = context;
    } */
    private readonly ReservationService _reservationService;

    public ReservationController(ReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    // CREAR RESERVA (Cálculo automático de precio)
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReservationDto reservationDto)
    {
        /*
        // Validar que la propiedad exista y traer su precio
        var property = await _context.Properties.FindAsync(reservationDto.PropertyId);
        
        if (property == null) return BadRequest("La propiedad no existe.");

        // Validar que el usuario exista
        var userExists = await _context.Users.AnyAsync(u => u.Id == reservationDto.UserId);
        if (!userExists) return BadRequest("El usuario no existe."); 

        // Calcular precio total (Noches * Precio por noche)
        var dias = (reservationDto.FechaFin - reservationDto.FechaInicio).Days;
        if (dias <= 0) return BadRequest("La fecha de fin debe ser posterior a la de inicio.");

        var nuevaReserva = new Reservation
        {
            FechaInicio = reservationDto.FechaInicio,
            FechaFin = reservationDto.FechaFin,
            PropertyId = reservationDto.PropertyId,
            UserId = reservationDto.UserId,
            PrecioTotal = dias * property.PrecioNoche, // Lógica automática
            EstadoReserva = 0, // Por ejemplo: 0 = Pendiente (usando tus Enums)
            FechaCreacion = DateTime.Now
        };

        _context.Reservations.Add(nuevaReserva);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Reserva realizada con éxito", total = nuevaReserva.PrecioTotal, reservaId = nuevaReserva.Id }); */
        // Convertimos el DTO a la Entidad
        var nuevaReserva = new Reservation
        {
            FechaInicio = reservationDto.FechaInicio,
            FechaFin = reservationDto.FechaFin,
            PropertyId = reservationDto.PropertyId,
            UserId = reservationDto.UserId
        };

        try 
        {
            // LLAMADA AL SERVICE: Aquí es donde ocurre la magia del email y el cálculo de precio

            // Aquí se valida, se calcula el precio y se envían los correos
            var resultado = await _reservationService.CrearReserva(nuevaReserva);

            return Ok(new { 
                message = "Reserva realizada con éxito", 
                total = resultado.PrecioTotal, 
                reservaId = resultado.Id 
            });
        }
        catch (Exception ex)
        {
            // Si el Service lanza una Exception (ej: "Propiedad no disponible"), cae aquí
            return BadRequest(ex.Message);
        }
    }

    // OBTENER TODAS (Para ver el historial)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetAll()
    {
       /* return await _context.Reservations
            .Include(r => r.property)
            .Include(r => r.user)
            .ToListAsync(); */
            return Ok(await _reservationService.ObtenerReservas());
    }

    [HttpPut("{id}/cancelar")]
    public async Task<IActionResult> Cancel(int id)
    {
        try 
        {
            await _reservationService.CancelarReserva(id);
            return Ok("Reserva cancelada y notificación enviada.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //Endpoint para completar y enviar correo de éxito
    [HttpPut("{id}/completar")]
    public async Task<IActionResult> Complete(int id)
    {
        try 
        {
            await _reservationService.CompletarReserva(id);
            return Ok("Reserva completada y notificación enviada.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    // CANCELAR RESERVA
    /*[HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        var reserva = await _context.Reservations.FindAsync(id);
        if (reserva == null) return NotFound();

        _context.Reservations.Remove(reserva);
        await _context.SaveChangesAsync();
        return Ok("Reserva cancelada.");
    } */
}
