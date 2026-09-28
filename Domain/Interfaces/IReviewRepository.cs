using Domain.Entities;

namespace Domain.Interfaces;

public interface IReviewRepository
{
    Task CrearResena(Review review);
    Task<List<Review>> ObtenerPorPropiedad(int propertyId);
    Task<Review?> ObtenerPorReservaId(int reservationId);
}