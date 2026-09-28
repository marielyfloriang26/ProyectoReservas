using Domain.Entities;

namespace Domain.Interfaces;
public interface IReservationRepository
{
    Task CrearReserva(Reservation reserva);
    Task<Reservation?> GetPorId(int id);
    Task<List<Reservation>> GetPorPropiedad(int propertyId);
    Task<List<Reservation>> GetPorUsuario(int userId);
    Task ActualizarReserva(Reservation reserva); // Necesario para cancelar/completar
    Task<List<Reservation>> GetTodas();
}