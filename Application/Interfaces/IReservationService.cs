using Application.DTOs;

namespace Application.Interfaces;
public interface IReservationService
{
    // Recibe un DTO de creación, devuelve un DTO de respuesta
    Task<ReservationDto> CrearReserva(ReservationDto dto, int userId);
    
    Task CancelarReserva(int reservationId, int userId);
    Task CompletarReserva(int reservationId, int userId);

    Task<List<ReservationDto>> ObtenerPorUsuario(int userId);
    Task<List<ReservationDto>> ObtenerPorPropiedad(int propertyId);
}