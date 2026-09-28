
namespace Application.Features.Concurrency;

public interface IConcurrencyHandler
{
    // Este método recibe el ID de lo que quiero bloquear (la propiedad)
    // y la función (el código) que quiero ejecutar de forma protegida.
    Task<bool> EstaDisponible(int propertyId, DateTime fechaInicio, DateTime fechaFin);
    
}