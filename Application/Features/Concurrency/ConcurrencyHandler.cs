using Domain.Interfaces;

namespace Application.Features.Concurrency;

public class ConcurrencyHandler : IConcurrencyHandler
{
    private readonly IReservationRepository _reservationRepository;

    // El "Semáforo" es estático para que todas las instancias del servicio
    // respeten el mismo portero. El (1, 1) significa: 1 persona entra, 0 esperan adentro.
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    public ConcurrencyHandler(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }
    public async Task<bool> EstaDisponible(int propertyId, DateTime fechaInicio, DateTime fechaFin)
    {
        //  PEDIR TURNO (EL CANDADO)
        // Si alguien ya está aquí, este hilo se queda esperando en esta línea.
        await _semaphore.WaitAsync();

        try
        {
            // CONSULTAR LA BASE DE DATOS
            // Traemos las reservas actuales de esa propiedad.
            var reservasExistentes = await _reservationRepository.GetPorPropiedad(propertyId);

            // (OVERLAPPING)
            // Una reserva choca si mi inicio es antes de su fin Y mi fin es después de su inicio.
            bool hayChoque = reservasExistentes.Any(r => fechaInicio < r.FechaFin && fechaFin > r.FechaInicio);

            // si hay choque devolvemos false (no disponible), sino true
            return !hayChoque;
        }
        catch (Exception)
        {
            // si algo explota, osea si la bd cae, se lanza el error pero 
            throw;
        }
        finally
        {
            // SOLTAR EL TURNO (ABRIR EL CANDADO)
            // El 'finally' garantiza que el candado se abra SIEMPRE,
            // incluso si hubo un error en el Paso 2.
            _semaphore.Release();
        }
    }
}